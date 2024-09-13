
using UnityEngine;

public class GameMap : MonoBehaviour
{
    private static GameMap Instance;

    [SerializeField] private TaskInfo Task;
    [SerializeField] private TaskDisplayer TaskDisplayer;

    [SerializeField] private MapLights MapLights;
    [SerializeField] private Floor[] Floors;

    private RoomsFloor[] RoomFloors;

    public static MapLights _MapLights => Instance.MapLights; 
    public static RoomsFloor[] _RoomFloors => Instance.RoomFloors;
    public static Floor[] _Floors => Instance.Floors;

    private void Awake()
    {
        Instance = this;

        RoomFloors = new RoomsFloor[0];
        foreach(Floor floor in Floors)
        {
            if(floor is RoomsFloor)
            {
                RoomFloors = StaticTools.ExpandMassive(RoomFloors, floor as RoomsFloor);
            }
        }

        TaskDisplayer.ApplyTask(Task, false);
    }

    public static void CheckClean()
    {
        int cleanFloors = 0;
        foreach(RoomsFloor floor in Instance.RoomFloors)
        {
            cleanFloors += floor._Clear.GetHashCode();
        }

        Instance.Task.Info = $"Убрать номера во всех этажах <b>{cleanFloors}/{Instance.RoomFloors.Length}</b>\nИли\nИзбавиться от монстров в отеле";
    }
}
