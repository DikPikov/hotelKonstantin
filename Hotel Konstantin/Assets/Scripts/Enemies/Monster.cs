
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, ILiftable
{
    [SerializeField] private NavMeshAgent NavMeshAgent;
    [SerializeField] private Animator Animator;
    [SerializeField] private Player Player;

    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private LayerMask PlayerMask;

    [SerializeField] private Behavior CurrentBehavior;

    [SerializeField] private Floor Floor;

    [SerializeField] private int[] CheckedRooms = new int[0];

    public Lift _Lift { get; set; }
    public Floor _Floor
    {
        get
        {
            return Floor;
        }
        set
        {
            Floor = value;
        }
    }
    public Transform _Transform => transform;

    public int _WalkAnimation => Game._HotelMadness > 0.25f ? 2 : 1;

    private void Update()
    {
        if (Pause._Paused)
        {
            return;
        }

        if(CurrentBehavior != null)
        {
            CurrentBehavior.Tick();
        }
        else
        {
            SetupBehavior(RandomWalk());
        }
    }

    public void SetupBehavior(Behavior behavior)
    {
        if(CurrentBehavior != null)
        {
            CurrentBehavior.Stop();
        }

        //print(behavior.GetType());

        CurrentBehavior = behavior;
        CurrentBehavior.Start();
    }

    public void RoomChecked(int room)
    {
        CheckedRooms = StaticTools.ExpandMassive(CheckedRooms, room);
    }

    public WalkBehavior RandomWalk()
    {
        int floor;
        int room = -1;

        if(Game._HotelMadness > 0.9f)
        {
            if(Floor == Player._Floor)
            {
                floor = StaticTools.IndexOf(GameMap._RoomFloors, Floor);

                if (Floor is RoomsFloor)
                {
                    Room[] rooms = (Floor as RoomsFloor)._Rooms;

                    int closest = 0;
                    float distance = Vector3.Distance(Player.transform.position, rooms[closest].transform.position);
                    for (int i = 1; i < rooms.Length; i++)
                    {
                        float newDistance = Vector3.Distance(Player.transform.position, rooms[i].transform.position);
                        if (distance > newDistance)
                        {
                            distance = newDistance;
                            closest = i;
                        }
                    }

                    room = closest;
                }
                else
                {
                    room = -1;
                }
            }
            else
            {
                floor = Random.Range(0, GameMap._RoomFloors.Length);
            }

            return new WalkBehavior(this, GameMap._RoomFloors[floor]._Index, room);
        }
        else if(Floor is RoomsFloor && CheckedRooms.Length < Random.Range(2, 6))
        {
            if (Game._HotelMadness > 0.25f)
            {
                floor = StaticTools.IndexOf(GameMap._RoomFloors, Floor);
            }
            else
            {
                //int[] openRooms = new int[0];
                //for(int i = 0; i < (Floor as RoomsFloor)._Rooms.Length; i++)
                //{

                //}
                CheckedRooms = new int[0];
                floor = Random.Range(0, GameMap._RoomFloors.Length);
            }
        }
        else
        {
            CheckedRooms = new int[0];
            floor = Random.Range(0, GameMap._RoomFloors.Length);
        }

        if (GameMap._Floors[floor] is RoomsFloor)
        {
            room = Random.Range(0, (GameMap._Floors[floor] as RoomsFloor)._Rooms.Length);
            while (StaticTools.Contains(CheckedRooms, room))
            {
                room = Random.Range(0, (GameMap._Floors[floor] as RoomsFloor)._Rooms.Length);
            }
        }

        return new WalkBehavior(this, GameMap._RoomFloors[floor]._Index, room);
    }

    [System.Serializable]
    public class Behavior
    {
        [SerializeField] protected Monster Monster;
        [SerializeField] protected string Info;

        protected virtual string _GetInfo => "";
        public string _Info => Info;

        public virtual void Start() { }

        public virtual void Tick() { }

        public virtual void Stop() { }
    }

    public class LookBehavior : Behavior
    {
        protected float CheckPlayerTimer = 0;

        public override void Tick()
        {
            if (Pause._Paused || Game._GameOver)
            {
                return;
            }

            CheckPlayerTimer -= Time.deltaTime;
            if (CheckPlayerTimer < 0)
            {
                CheckPlayerTimer = 0.25f;

                if (Monster.Player._Lift != null && Monster.Player._Lift._Moves)
                {
                    return;
                }

                RaycastHit hit;
                if (Physics.Raycast(Monster.transform.position + Vector3.up * 0.5f, Monster.Player.transform.position - Monster.transform.position, out hit, 100, Monster.LayerMask))
                {
                    if (hit.transform == Monster.Player.transform)
                    {
                        Monster.SetupBehavior(new RunAfterBehavior(Monster));
                        return;
                    }
                }
            }
        }
    }

    public class WalkBehavior : LookBehavior
    {
        private int Floor;
        private int Room;

        private Lift Lift = null;

        private bool Moved = false;

        public WalkBehavior(Monster monster,  int floor, int room)
        {
            Monster = monster;
            Floor = floor;
            Room = room;

            Info = $"Прийти на {floor} этаж, {room} комнату";
        }

        public override void Start()
        {
            Monster.NavMeshAgent.speed = 2.5f + 2.5f * Game._HotelMadness;
            Monster.NavMeshAgent.angularSpeed = 120;

            if (Floor != Monster.Floor._Index)
            {
                Vector3 lift1 = GameMap._Lifts[0].transform.position;
                Vector3 lift2 = GameMap._Lifts[1].transform.position;
                lift1.y = Monster.transform.position.y;
                lift2.y = Monster.transform.position.y;

                if (Vector3.Distance(Monster.transform.position, lift1) < Vector3.Distance(Monster.transform.position, lift2))
                {
                    Lift = GameMap._Lifts[0];
                }
                else
                {
                    Lift = GameMap._Lifts[1];
                }
            }

            if (Lift == null)
            {
                if (Room > -1)
                {
                    Monster.NavMeshAgent.SetDestination((GameMap._Floors[Floor] as RoomsFloor)._Rooms[Room].transform.position);
                }
                else
                {
                    Vector3 lift1 = GameMap._Lifts[0].transform.position;
                    Vector3 lift2 = GameMap._Lifts[1].transform.position;
                    lift1.y = Monster.transform.position.y;
                    lift2.y = Monster.transform.position.y;

                    if (Vector3.Distance(Monster.transform.position, lift1) > Vector3.Distance(Monster.transform.position, lift2))
                    {
                        Monster.NavMeshAgent.SetDestination(lift1);
                    }
                    else
                    {
                        Monster.NavMeshAgent.SetDestination(lift2);
                    }
                }
            }
            else
            {
                Monster.NavMeshAgent.SetDestination(Lift.transform.position + Lift.transform.forward * 2);
            }
        }

        public override void Tick()
        {
            if (!Moved)
            {
                if (!Monster.NavMeshAgent.pathPending)
                {
                    Moved = true;
                }
                return;
            }

            Monster.Animator.SetInteger("MoveState", Monster._WalkAnimation);

            if (!Monster.NavMeshAgent.hasPath)
            {
                if(Lift != null)
                {
                    Monster.SetupBehavior(new OrderLiftBehavior(Monster, Lift, Floor, Room));
                }
                else
                {
                    Monster.RoomChecked(Room);

                    Monster.SetupBehavior(Monster.RandomWalk());
                }

                return;
            }
            else
            {
                RaycastHit hit;
                if(Physics.Raycast(Monster.transform.position + Vector3.up, Monster.NavMeshAgent.velocity, out hit, Monster.NavMeshAgent.velocity.magnitude - 1, Monster.LayerMask))
                {
                    Door door = hit.transform.GetComponentInParent<Door>();
                    if(door != null && !door._Opened)
                    {
                        Monster.SetupBehavior(new ClosedDoorBehavior(Monster, door, Room));
                    }
                }
            }

            base.Tick();
        }

        public override void Stop()
        {
            base.Stop();

            Monster.NavMeshAgent.velocity = Vector3.zero;
            Monster.NavMeshAgent.destination = Monster.transform.position;

            Monster.Animator.SetInteger("MoveState", 0);
        }
    }

    public class OrderLiftBehavior : LookBehavior
    {
        private int Floor;
        private int Room;

        private Lift Lift;

        private float Delay = 0;

        private byte BehaviorState = 0; //0 - call and wait lift      1 - get in lift     2 - wait lifted       2 - leave lift after elevating

        public OrderLiftBehavior(Monster monster, Lift lift, int floor, int room)
        {
            Info = $"Подняться на {floor} этаж";

            Lift = lift ;
            Monster = monster;
            Floor= floor;
            Room = room;
        }

        public override void Start()
        {
            if (Monster.GetComponentInParent<Lift>() == Lift)
            {
                BehaviorState = 2;
            }
            else if (!Lift._Moves)
            {
                if (Lift._Floor == Monster._Floor._Index)
                {
                    BehaviorState = 1;
                }
                else
                {
                    Lift.Elevate(Monster._Floor._Index);
                }
            }
        }

        public override void Tick()
        {
            switch (BehaviorState)
            {
                case 0:
                    {
                        Monster.Animator.SetInteger("MoveState", 0);

                        if (!Lift._Moves)
                        {
                            if (Lift._Floor == Monster._Floor._Index)
                            {
                                Monster.transform.position = Lift.transform.position + Lift.transform.forward * 1.5f;
                                Monster.transform.localEulerAngles = Lift.transform.rotation.eulerAngles + Vector3.up * 180;

                                Monster.Animator.Play("OpenLift");
                                Delay = 0.8f;

                                Lift.AnimateBreaking();

                                Monster.NavMeshAgent.enabled = false;

                                BehaviorState = 1;
                            }
                            else
                            {
                                Lift.Elevate(Monster._Floor._Index);
                            }
                        }

                        base.Tick();
                    }
                    break;
                case 1:
                    {
                        if(Delay > 0)
                        {
                            Delay -= Time.deltaTime;
                            break;
                        }
                        Monster.Animator.SetInteger("MoveState", Monster._WalkAnimation);

                        Monster.NavMeshAgent.enabled = false;

                        float distance = (Lift.transform.position - Monster.transform.position).magnitude;

                        Monster.transform.position += (Lift.transform.position - Monster.transform.position) / distance * Time.deltaTime * Monster.NavMeshAgent.speed / 2;

                        if (distance < Vector3.Distance(Lift.transform.position, Monster.transform.position))
                        {
                            Monster.transform.position = Lift.transform.position;
                            Monster.transform.rotation = Lift.transform.rotation;

                            BehaviorState = 2;
                        }

                        base.Tick();
                    }
                    break;
                case 2:
                    {
                        Monster.Animator.SetInteger("MoveState", 0);

                        if (!Lift._Moves)
                        {
                            if (Lift._Floor == Floor)
                            {
                                BehaviorState = 3;

                                Monster.Animator.Play("OpenLift");
                                Delay = 0.75f;

                                Lift.AnimateBreaking();

                                Monster.Animator.SetInteger("MoveState", Monster._WalkAnimation);
                            }
                            else
                            {
                                Lift.Elevate(Floor);
                            }
                        }
                    }
                    break;
                case 3:
                    {
                        if (Delay > 0)
                        {
                            Delay -= Time.deltaTime;
                            break;
                        }

                        Monster.Animator.SetInteger("MoveState", Monster._WalkAnimation);

                        Monster.NavMeshAgent.enabled = true;

                        if (!Monster.NavMeshAgent.isOnNavMesh)
                        {
                            Monster.transform.position += (GameMap._Floors[Floor].transform.position + Vector3.up - Monster.transform.position).normalized * Time.deltaTime * Monster.NavMeshAgent.speed / 2 ;
                        }
                        else
                        {

                            Monster.SetupBehavior(new WalkBehavior(Monster, Floor, Room));
                            return;
                        }
                    }
                    break;
            }
        }

        public override void Stop()
        {
            base.Stop();

            switch (BehaviorState)
            {
                case 0:
                    Monster.Animator.SetInteger("MoveState", 0);
                    break;
                case 1:
                    Monster.transform.position = Lift.transform.position + Lift.transform.forward * 2;
                    Monster.NavMeshAgent.enabled = true;
                    break;
            }

            Monster.Animator.SetInteger("MoveState", 0);
        }
    }

    public class ClosedDoorBehavior : LookBehavior
    {
        private Door Door;
        private int Room;

        private float Delay = 0;

        private bool Nomad = false;

        public ClosedDoorBehavior(Monster monster, Door door)
        {
            Monster = monster;
            Door = door;

            Room = -228;
        }

        public ClosedDoorBehavior(Monster monster, Door door, int room)
        {
            Monster = monster;
            Room = room;
            Door = door;
        }

        public override void Start()
        {
            if (Vector3.Distance(Monster.transform.position, Door._Points[0].position) > Vector3.Distance(Monster.transform.position, Door._Points[1].position))
            {
                Monster.transform.position = Door._Points[1].position;
                Monster.transform.rotation = Door._Points[1].rotation;
            }
            else
            {
                Monster.transform.position = Door._Points[0].position;
                Monster.transform.rotation = Door._Points[0].rotation;
            }

            if (Game._HotelMadness > 0.5f)
            {
                Delay = 0.3f;
                Monster.Animator.Play("BreakDoor");
                Door._Opened = true;
                Door.PlayBreaking();
            }
            else if (Game._HotelMadness > 0.25f)
            {
                Delay = 1;
                Monster.Animator.Play("OpenDoor");
                Door._Opened = true;
            }
            else
            {
                Delay = 0.1f;
                Nomad = true;
            }
        }

        public override void Tick()
        {
            if(Delay > 0)
            {
                Monster.NavMeshAgent.isStopped = true;
                Delay -= Time.deltaTime;
                return;
            }

            if (Nomad)
            {
                Monster.NavMeshAgent.isStopped = true;
                Monster.Animator.SetInteger("MoveState", 0);

                Door.SetBarriering(true);
                if (Game._HotelMadness < 0.25f)
                {
                    return;
                }

                Delay = 1;
                Monster.Animator.Play("BreakDoor");
                Door._Opened = true;
                Door.PlayBreaking();

                Door.SetBarriering(false);
            }

            Monster.NavMeshAgent.isStopped = false;

            if(Room == -228)
            {
                Monster.SetupBehavior(new RunAfterBehavior(Monster));
            }
            else
            {
                Monster.SetupBehavior(new WalkBehavior(Monster, Monster._Floor._Index, Room));
            }

            base.Tick();
        }

        public override void Stop()
        {
            Door.SetBarriering(false);
            Monster.NavMeshAgent.isStopped = false;
        }
    }

    public class RunAfterBehavior : Behavior
    {
        private float Delay = 0;

        public RunAfterBehavior(Monster monster)
        {
            Info = $"Преследовать игрока";
            Monster = monster;

        }

        public override void Start()
        {
            Monster.NavMeshAgent.speed = 3f + 5f * Game._HotelMadness;
            Monster.NavMeshAgent.angularSpeed = 360;

        }

        public override void Tick()
        {
            if(Delay > 0)
            {
                Delay -= Time.deltaTime;
                return;
            }

            if(Monster.Player._Floor != Monster.Floor)
            {
                Monster.SetupBehavior(Monster.RandomWalk());
                return;
            }

            if (Monster.Player._Lift != null)
            {
                if (Monster.Player._Lift._Moves)
                {
                    if(Game._HotelMadness > 0.25f)
                    {
                        if (Monster.Player._Lift._TargetFloor < Monster._Floor._Index)
                        {
                            Monster.SetupBehavior(new WalkBehavior(Monster, Random.Range(0, Monster._Floor._Index), -1));
                        }
                        else
                        {
                            Monster.SetupBehavior(new WalkBehavior(Monster, Random.Range(Monster._Floor._Index + 1, GameMap._Floors.Length), -1));
                        }
                    }
                    else
                    {
                        Monster.SetupBehavior(Monster.RandomWalk());
                    }
                    return;
                }
                if(Vector3.Distance(Monster.transform.position, Monster.Player._Lift.transform.position) < 1.5f)
                {
                    if (!Monster.Player._Lift._Open)
                    {
                        if(Game._HotelMadness > 0.5f)
                        {

                            Monster.Player._Lift.AnimateBreaking();

                            Monster.Animator.Play("OpenLift");
                            Delay = 0.8f;
                        }
                        else
                        {
                            Monster.SetupBehavior(Monster.RandomWalk());
                        }
                        return;
                    }

                    Monster.transform.position = Monster.Player._Lift.transform.position + Monster.Player._Lift.transform.forward * 0.8f;

                    Monster.Animator.Play($"Attack{Random.Range(1, 4)}");

                    Game._GameOver = true;

                    Monster.SetupBehavior(new StayBehavior(Monster));

                    return;
                }
            }

            Monster.Animator.SetInteger("MoveState", Game._HotelMadness < 0.5f ? Monster._WalkAnimation : 3);
            Monster.NavMeshAgent.destination = Monster.Player.transform.position;

            RaycastHit hit;
            if (Physics.Raycast(Monster.transform.position + Vector3.up, Monster.transform.forward, out hit, 2, Monster.LayerMask))
            {
                Door door = hit.transform.GetComponentInParent<Door>();
                if (door != null && !door._Opened)
                {
                    Monster.SetupBehavior(new ClosedDoorBehavior(Monster, door));
                }
                else if (hit.transform.GetComponent<Player>() && hit.distance < 0.8f)
                {
                    Monster.NavMeshAgent.isStopped = true;
                    Monster.Animator.Play($"Attack{Random.Range(1, 4)}");

                    Game._GameOver = true;

                    Monster.SetupBehavior(new StayBehavior(Monster));
                }
            }
            else
            {
                foreach(Collider collider in Physics.OverlapSphere(Monster.transform.position + Vector3.up, 0.1f, Monster.PlayerMask))
                {
                    if (collider.transform.GetComponent<Player>())
                    {
                        Monster.NavMeshAgent.isStopped = true;
                        Monster.Animator.Play($"Attack{Random.Range(1, 4)}");

                        Game._GameOver = true;

                        Monster.SetupBehavior(new StayBehavior(Monster));

                        break;
                    }
                }
            }
        }

        public override void Stop()
        {
            base.Stop();

            Monster.Animator.SetInteger("MoveState", 0);
            Monster.NavMeshAgent.velocity = Vector3.zero;
            Monster.NavMeshAgent.destination = Monster.transform.position;
            Monster.NavMeshAgent.isStopped = false;
        }
    }

    public class StayBehavior : Behavior
    {
        public StayBehavior(Monster monster)
        {
            Monster = monster;
        }

        public override void Start()
        {
            if (!Monster.NavMeshAgent.enabled)
            {
                Monster.NavMeshAgent.enabled = true;
            }

            Monster.NavMeshAgent.velocity= Vector3.zero;
            Monster.NavMeshAgent.destination = Monster.transform.position;

            Monster.Animator.SetInteger("MoveState", 0);

            base.Start();
        }
    }
}
