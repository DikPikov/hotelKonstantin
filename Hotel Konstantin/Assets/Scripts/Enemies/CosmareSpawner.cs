using UnityEngine;
using UnityEngine.UI;

public class CosmareSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] CosmarePrefab;
    [SerializeField] private Player Player;
    [SerializeField] private Image CosmareNoise;
    [SerializeField] private float SpawnTime;

    private Cosmare Cosmare = null;

    private void Start()
    {
        SpawnTime = Random.Range(20, 70f);
    }

    private void Update()
    {
        if(SpawnTime > 0)
        {
            SpawnTime -= Time.deltaTime;
        }
        else
        {
            if(Cosmare != null)
            {
                Destroy(Cosmare.gameObject);
            }

            Cosmare = Instantiate(CosmarePrefab[Random.Range(0, CosmarePrefab.Length)], Player._Floor.GetSpawnPoint(), transform.rotation).GetComponent<Cosmare>();
            Cosmare.SetInfo(Player, CosmareNoise);

            SpawnTime = Random.Range(20, 70f - 40 * Game._HotelMadness);
        }
    }
}
