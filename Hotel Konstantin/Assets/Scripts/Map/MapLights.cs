
using UnityEngine;

public class MapLights : MonoBehaviour
{
    [SerializeField] private float[] FloorsLightTime;
    [SerializeField] private byte[] FloorsState; // 0 - on, 1 - distort, 2 - off
    [SerializeField] private FuseSwitch[] FloorsFuses;

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

        FloorsFuses[1].SetFuseNoNotify(new DamagedFuse());

        int[] used = new int[] { 0, 1 };
        for(int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, FloorsFuses.Length);
            while(StaticTools.IndexOf(used, random) > -1)
            {
                random = Random.Range(0, FloorsFuses.Length);
            }

            used = StaticTools.ExpandMassive(used, random);
            FloorsFuses[random].SetFuseNoNotify(new DamagedFuse());
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
                FloorsLightTime[index] = Time.time + Random.Range(60f, 120f - 60 * Game._HotelMadness);

                GameMap._Floors[index]._Light.Enable(false);
                break;
        }
    }

    public void FloorFuseUpdate(int floor)
    {
        switch (FloorsFuses[floor]._State)
        {
            case 0:
                FloorsLightTime[floor] = Mathf.Infinity;

                FloorsState[floor] = 2;
                GameMap._Floors[floor]._Light.Enable(false);
                break;
            case 1:
                SetFloorState(floor, 1);
                break;
            case 2:
                FloorsLightTime[floor] = Mathf.Infinity;

                FloorsState[floor] = 0;
                GameMap._Floors[floor]._Light.Enable(true);
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
