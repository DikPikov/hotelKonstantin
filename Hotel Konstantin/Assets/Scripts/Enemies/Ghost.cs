using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Agent;

    private Player Player;
    private Floor Floor;
    private Image GhostNoise;

    private void OnDestroy()
    {
        GhostNoise.color = new Color(0.3f, 0.3f, 1, 0);
    }

    public void SetInfo(Player player, Floor floor, Image noise)
    {
        Player = player;

        Player.OnFloorChange += PlayerChangedFloor;

        Floor = floor; 

        GhostNoise = noise;

        Agent.speed = 1 + Game._HotelMadness * 4;
    }

    private void PlayerChangedFloor()
    {

    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        GhostNoise.color = new Color(0.3f, 0.3f, 1, 1 - Mathf.Clamp01(distance * 0.1f));
        Player._Sanity -= Time.deltaTime * 0.01f;

        Agent.destination = Player.transform.position;

        if(distance < 0.65f)
        {
            Game._HotelTime += Random.Range(300, 900);
            Player._Sanity += 0.25f - 0.25f * Game._HotelMadness;
            Destroy(gameObject);

            GhostNoise.color = new Color(0.3f, 0.3f, 1, 0);
        }
    }
}
