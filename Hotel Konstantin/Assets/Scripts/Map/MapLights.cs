using UnityEngine;

public class MapLights : MonoBehaviour
{
    [SerializeField] private float[] FloorsLightTime;
    [SerializeField] private byte[] FloorsState; // 0 - on, 1 - distort, 2 - off

    private void Start()
    {
        FloorsLightTime = new float[GameMap._Floors.Length];
        FloorsState = new byte[FloorsLightTime.Length];

        FloorsLightTime[0] = Mathf.Infinity;
        FloorsLightTime[1] = Time.time + Random.Range(60f, 180f - 90 * Game._HotelMadness);

        for (int i = 2; i < FloorsLightTime.Length; i++)
        {
            float random = Random.Range(-180f, 180);

            FloorsLightTime[i] = Time.time + Mathf.Abs(random);
            if (random > 0)
            {
                FloorsState[i] = 0;

                GameMap._Floors[i]._Light.Enable(true);
            }
            else
            {
                FloorsState[i] = 2;

                GameMap._Floors[i]._Light.Enable(false);
            }
        }
    }

    private void Update()
    {
        for(int i = 0; i < FloorsLightTime.Length; i++)
        {
            if (FloorsLightTime[i] <= Time.time)
            {
                switch (FloorsState[i])
                {
                    case 0:
                        FloorsLightTime[i] = Time.time + Random.Range(10f, 30 - 15 * Game._HotelMadness);
                        FloorsState[i] = 1;

                        GameMap._Floors[i]._Light.DistortLight();
                        break;
                    case 1:
                        FloorsLightTime[i] = Time.time + Random.Range(180, 300f - 120 * Game._HotelMadness);
                        FloorsState[i] = 2;

                        GameMap._Floors[i]._Light.Enable(false);
                        break;
                    case 2:
                        FloorsLightTime[i] = Time.time + Random.Range(60f, 180f - 90 * Game._HotelMadness);
                        FloorsState[i] = 0;

                        GameMap._Floors[i]._Light.Enable(true);
                        break;
                }
            }
        }
    }
}
