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
        FloorsLightTime[1] = Time.time + Random.Range(60f, 120f - 60 * Game._HotelMadness);

        for (int i = 2; i < FloorsLightTime.Length; i++)
        {
            float random = Random.Range(-90f, 120);

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

    public void SetFloorState(int index, byte state)
    {
        FloorsState[index] = state;

        switch (state)
        {
            case 0:
                FloorsLightTime[index] = Time.time + Random.Range(60f, 120f - 60 * Game._HotelMadness);

                GameMap._Floors[index]._Light.Enable(true);
                break;
            case 1:
                FloorsLightTime[index] = Time.time + Random.Range(10f, 30 - 15 * Game._HotelMadness);

                GameMap._Floors[index]._Light.DistortLight();
                break;
            case 2:
                FloorsLightTime[index] = Time.time + Random.Range(90f, 90f + 60 * Game._HotelMadness);

                GameMap._Floors[index]._Light.Enable(false);
                break;
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
                        SetFloorState(i, 1);
                        break;
                    case 1:
                        SetFloorState(i, 2);
                        break;
                    case 2:
                        SetFloorState(i, 0);
                        break;
                }
            }
        }
    }
}
