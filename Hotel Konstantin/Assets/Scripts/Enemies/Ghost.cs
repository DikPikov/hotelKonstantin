using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class Ghost : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Animator Animator;

    private Player Player;
    private Floor Floor;
   // private Image GhostNoise;

    private void OnDestroy()
    {
      //  GhostNoise.color = new Color(0.3f, 0.3f, 1, 0);
    }

    public void SetInfo(Player player, Floor floor)
    {
        Player = player;

        Player.OnFloorChange += PlayerChangedFloor;

        Floor = floor; 

      //  GhostNoise = noise;

        Agent.speed = 2 + Game._HotelMadness * 3;
    }

    private void PlayerChangedFloor()
    {

    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

      //  GhostNoise.color = new Color(0.3f, 0.3f, 1, 1 - Mathf.Clamp01(distance * 0.1f));

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

            int floor = Random.Range(0, GameMap._Floors.Length);
            int room = Random.Range(0, GameMap._Floors[floor]._Rooms.Length);

            Player.transform.position = GameMap._Floors[floor]._Rooms[room].transform.position + Vector3.up;
            FindObjectOfType<MapLights>().SetFloorState(floor, 0);

            Game._HotelTime += Random.Range(300, 900);
            Player._Sanity += 0.25f - 0.25f * Game._HotelMadness;

         //   GhostNoise.color = new Color(0.3f, 0.3f, 1, 0);
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
