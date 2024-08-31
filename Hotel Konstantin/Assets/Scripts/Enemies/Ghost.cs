using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Agent;

    private Player Player;
    private CoridorLights CoridorLights;

    public void SetInfo(Player player, CoridorLights lights)
    {
        Player = player;
        CoridorLights = lights;
    }

    private void Update()
    {
        Player._Sanity -= Time.deltaTime * 0.01f;

        Agent.destination = Player.transform.position;

        if(Vector3.Distance(transform.position, Player.transform.position) < 0.5f)
        {
            Player._Sanity += 0.25f;
            CoridorLights._LightTime = 60;
            Destroy(gameObject);
        }
    }
}
