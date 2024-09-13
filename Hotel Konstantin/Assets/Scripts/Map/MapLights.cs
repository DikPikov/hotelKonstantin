
using UnityEngine;

public class MapLights : MonoBehaviour
{
    [SerializeField] private float[] FloorsLightTime;
    [SerializeField] private byte[] FloorsState; // 0 - on, 1 - distort, 2 - off
    [SerializeField] private FuseSwitch[] FloorsFuses;

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

        FloorsFuses[1].SetFuseNoNotify(new DamagedFuse());

        int[] used = new int[] { 0 };
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

        for(int i = 0; i < GameMap._RoomFloors.Length; i++)
        {
            if(!StaticTools.Contains(used, i))
            {
                FloorsFuses[i]._Fuse = null;
            }
        }
    }

    public void SetFloorState(RoomsFloor floor, byte state)
    {
        FloorsState[Index(floor)] = state;

        switch (state)
        {
            case 0:
                FloorsLightTime[Index(floor)] = Time.time + Random.Range(60f, 120f - 60 * Game._HotelMadness);

                GameMap._RoomFloors[Index(floor)]._Light.Enable(true);
                break;
            case 1:
                FloorsLightTime[Index(floor)] = Time.time + Random.Range(10f, 30 - 15 * Game._HotelMadness);

                GameMap._RoomFloors[Index(floor)]._Light.DistortLight();
                break;
            case 2:
                FloorsLightTime[Index(floor)] = Time.time + Random.Range(60f, 120f - 60 * Game._HotelMadness);

                GameMap._RoomFloors[Index(floor)]._Light.Enable(false);
                break;
        }
    }

    public void FloorFuseUpdate(RoomsFloor floor)
    {
        switch (FloorsFuses[StaticTools.IndexOf(GameMap._RoomFloors, floor)]._State)
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
