
using UnityEngine;

public class MapLights : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private AudioSource LightDistorting;

    [SerializeField] private float[] FloorsLightTime;
    [SerializeField] private byte[] FloorsState; // 0 - on, 1 - distort, 2 - off

    private int Index(RoomsFloor floor) => StaticTools.IndexOf(GameMap._RoomFloors, floor);

    private void Start()
    {
        FloorsLightTime = new float[GameMap._RoomFloors.Length];
        FloorsState = new byte[FloorsLightTime.Length];

        FloorsLightTime[0] = Time.time + Random.Range(60f, 120f - 60 * Game._HotelMadness);

        for (int i = 1; i < FloorsLightTime.Length; i++)
        {
            float random = Random.Range(-90f, 120);

            FloorsLightTime[i] = Time.time + Mathf.Abs(random);
            if (random > 0)
            {
                FloorsState[i] = 0;

                GameMap._RoomFloors[i]._Light.Enable(true);
            }
            else
            {
                FloorsState[i] = 2;

                GameMap._RoomFloors[i]._Light.Enable(false);
            }
        }

        GameMap._BasementFloor.InitializeFuses();
    }

    public int GetFloorState(Floor floor)
    {
        if(floor is RoomsFloor)
        {
            return FloorsState[Index(floor as RoomsFloor)];
        }

        return -1;
    }

    public void SetFloorState(RoomsFloor floor, byte state)
    {
        FloorsState[Index(floor)] = state;

        switch (state)
        {
            case 0:
                FloorsLightTime[Index(floor)] = Time.time + Random.Range(60f, 120f - 60 * Game._HotelMadness);

                GameMap._RoomFloors[Index(floor)]._Light.Enable(true);

                if (Player._Floor == floor)
                {
                    LightDistorting.volume = 0;
                }
                break;
            case 1:
                FloorsLightTime[Index(floor)] = Time.time + Random.Range(10f, 30 - 15 * Game._HotelMadness);

                GameMap._RoomFloors[Index(floor)]._Light.DistortLight();

                if (Player._Floor == floor)
                {
                    LightDistorting.volume = 1;
                }
                break;
            case 2:
                FloorsLightTime[Index(floor)] = Time.time + Random.Range(60f, 120f - 60 * Game._HotelMadness);

                GameMap._RoomFloors[Index(floor)]._Light.Enable(false);

                if(Player._Floor == floor)
                {
                    LightDistorting.volume = 0;
                }
                break;
        }
    }

    public void FloorFuseUpdate(RoomsFloor floor, byte state)
    {
        switch (state)
        {
            case 0:
                FloorsLightTime[Index(floor)] = Mathf.Infinity;

                FloorsState[Index(floor)] = 2;
                GameMap._RoomFloors[Index(floor)]._Light.Enable(false);
                break;
            case 1:
                SetFloorState(floor, 1);
                break;
            case 2:
                FloorsLightTime[Index(floor)] = Mathf.Infinity;

                FloorsState[Index(floor)] = 0;
                GameMap._RoomFloors[Index(floor)]._Light.Enable(true);
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
                        SetFloorState(GameMap._RoomFloors[i], 1);
                        break;
                    case 1:
                        SetFloorState(GameMap._RoomFloors[i], 2);
                        break;
                    case 2:
                        SetFloorState(GameMap._RoomFloors[i], 0);
                        break;
                }
            }
        }
    }
}
