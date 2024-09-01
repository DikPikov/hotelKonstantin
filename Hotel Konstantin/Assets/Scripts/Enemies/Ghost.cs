using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Agent;

    private Player Player;
    private CoridorLights CoridorLights;
    private Image GhostNoise;

    public void SetInfo(Player player, CoridorLights lights, Image noise)
    {
        Player = player;
        CoridorLights = lights;
        GhostNoise = noise;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        GhostNoise.color = new Color(0.3f, 0.3f, 1, 1 - Mathf.Clamp01(distance * 0.1f));
        Player._Sanity -= Time.deltaTime * 0.01f;

        Agent.destination = Player.transform.position;

        if(distance < 0.5f)
        {
            Game._HotelTime += 3600;
            Player._Sanity += 0.25f;
            CoridorLights._LightTime = 60;
            Destroy(gameObject);

            GhostNoise.color = new Color(0.3f, 0.3f, 1, 0);
        }
    }
}
