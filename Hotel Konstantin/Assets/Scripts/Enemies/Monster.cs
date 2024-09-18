using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;

public class Monster : MonoBehaviour, ILiftable
{
    [SerializeField] private NavMeshAgent NavMeshAgent;
    [SerializeField] private Animator Animator;
    [SerializeField] private Player Player;

    [SerializeField] private LayerMask LayerMask;

    [SerializeField] private Behavior CurrentBehavior;

    [SerializeField] private Floor Floor;

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

    private void Start()
    {
        SetupBehavior(new WalkBehavior(this, 1, 0));
    }

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

    private void SetupBehavior(Behavior behavior)
    {
        if(CurrentBehavior != null)
        {
            CurrentBehavior.Stop();
        }

        print(behavior.GetType());

        CurrentBehavior = behavior;
        CurrentBehavior.Start();
    }

    private WalkBehavior RandomWalk()
    {
        int floor = Random.Range(0, GameMap._RoomFloors.Length);
        int room = -1;

        return new WalkBehavior(this, GameMap._RoomFloors[floor]._Index, room);
    }

    [System.Serializable]
    private class Behavior
    {
        [SerializeField] protected Monster Monster;
        [SerializeField] protected string Info;

        protected virtual string _GetInfo => "";
        public string _Info => Info;

        public virtual void Start() { }

        public virtual void Tick() { }

        public virtual void Stop() { }
    }

    private class LookBehavior : Behavior
    {
        protected float CheckPlayerTimer = 0;

        public override void Tick()
        {
            CheckPlayerTimer -= Time.deltaTime;
            if (CheckPlayerTimer < 0)
            {
                CheckPlayerTimer = 0.25f;

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

    private class WalkBehavior : LookBehavior
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
                print(Monster.NavMeshAgent.pathPending);
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

    private class OrderLiftBehavior : Behavior
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
                                Monster.transform.position = Lift.transform.position + Lift.transform.forward * 2;
                                Monster.transform.localEulerAngles = Lift.transform.rotation.eulerAngles + Vector3.up * 180;

                                Monster.Animator.Play("OpenLift");
                                Delay = 0.8f;

                                Lift.AnimateBreaking();

                                BehaviorState = 1;
                            }
                            else
                            {
                                Lift.Elevate(Monster._Floor._Index);
                            }
                        }
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

            base.Tick();
        }

        public override void Stop()
        {
            base.Stop();

            Monster.Animator.SetInteger("MoveState", 0);
        }
    }

    private class ClosedDoorBehavior : LookBehavior
    {
        private Door Door;
        private int Room;

        private float Delay = 0;

        private bool Nomad = false;

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

            Monster.SetupBehavior(new WalkBehavior(Monster, Monster._Floor._Index, Room));

            base.Tick();
        }

        public override void Stop()
        {
            Door.SetBarriering(false);
            Monster.NavMeshAgent.isStopped = false;
        }
    }

    private class RunAfterBehavior : Behavior
    {
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
            Monster.Animator.SetInteger("MoveState", Game._HotelMadness < 0.5f ? Monster._WalkAnimation : 3);
            Monster.NavMeshAgent.destination = Monster.Player.transform.position;
        }
    }
}
