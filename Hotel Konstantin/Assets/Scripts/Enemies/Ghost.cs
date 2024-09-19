using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class Ghost : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Animator Animator;
    [SerializeField] private Transform Head;
    [SerializeField] private AudioSource[] Walks;

    private Transform Camera = null;

    private Player Player;
    private Floor Floor;
    private Image GhostNoise;

    private void OnDestroy()
    {
       if(Player._Floor == Floor)
        {
            GhostNoise.color = new Color(0.3f, 0.3f, 1, 0);
        }

        Player.OnFloorChange -= PlayerChangedFloor;
    }

    public void PlayWalkSound()
    {
        int sound = Random.Range(0, Walks.Length);

        Walks[sound].pitch = Random.Range(0.9f, 1.1f);
        Walks[sound].Play();
    }

    public void SetInfo(Player player, Floor floor, Image noise)
    {
        Player = player;

        Camera = Player.GetComponentInChildren<Camera>().transform;

        Player.OnFloorChange += PlayerChangedFloor;

        Floor = floor; 

        GhostNoise = noise;

        Agent.speed = 2 + Game._HotelMadness * 3;

        PlayerChangedFloor();
    }

    private void PlayerChangedFloor()
    {
        GhostNoise.color = new Color(0.3f, 0.3f, 1, 0);

        if (Player._Floor != Floor)
        {
            Animator.SetBool("Moving", false);

            if (Agent.isActiveAndEnabled)
            {
                Agent.isStopped = true;
            }
        }
        else if (Agent.isActiveAndEnabled)
        {
            Agent.isStopped = false;
        }
    }

    private void Update()
    {
        if (Pause._Paused)
        {
            return;
        }

        if (Agent.isStopped)
        {
            return;
        }

        Head.LookAt(Camera);

        float distance = Vector3.Distance(transform.position, Player.transform.position);

        GhostNoise.color = new Color(0.3f, 0.3f, 1, 1 - Mathf.Clamp01(distance * 0.1f));

        Agent.destination = Player.transform.position;

        if(Agent.velocity == Vector3.zero)
        {
            Animator.SetBool("Moving", false);
        }
        else
        {
            Animator.SetBool("Moving", true);
        }

        if(distance < 0.65f)
        {
            Destroy(gameObject);

            if (Game._HotelMadness == 1)
            {
                StartCoroutine(Kill());
                return;
            }

            bool hasFloor = false;
            for(int i = 0; i < GameMap._RoomFloors.Length; i++)
            {
                if(GameMap._BasementFloor._Fuses[i]._State != 0)
                {
                    hasFloor = true;
                    break;
                }
            }

            if (hasFloor)
            {
                int floor = Random.Range(0, GameMap._RoomFloors.Length);

                while (GameMap._BasementFloor._Fuses[floor]._State == 0)
                {
                    floor = Random.Range(0, GameMap._RoomFloors.Length);
                }

                int room = Random.Range(0, GameMap._RoomFloors[floor]._Rooms.Length);

                GameMap._RoomFloors[floor]._Rooms[room]._RoomLight._Enabled = true;
                Player.transform.position = GameMap._RoomFloors[floor]._Rooms[room].transform.position + Vector3.up;
                Player._Floor = GameMap._RoomFloors[floor];
                FindObjectOfType<MapLights>().SetFloorState(GameMap._RoomFloors[floor], 0);
            }
            else
            {
                Player.transform.position = GameMap._BasementFloor.transform.position + Vector3.up;
                Player._Floor = GameMap._BasementFloor;
            }

            Game._HotelTime += Random.Range(300, 900);

            GhostNoise.color = new Color(0.3f, 0.3f, 1, 0);
        }
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSecondsRealtime(1);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
