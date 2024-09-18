using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
public class GameMap : MonoBehaviour
{
    private static GameMap Instance;

    [SerializeField] private TaskInfo Task;
    [SerializeField] private TaskDisplayer TaskDisplayer;

    [SerializeField] private MapLights MapLights;
    [SerializeField] private Floor[] Floors;

    [SerializeField] private Lift[] Lifts;
    [SerializeField] private GameObject Manager;

    private BasementFloor Basement = null;
    private RoomsFloor[] RoomFloors;

    public static MapLights _MapLights => Instance.MapLights;
    public static BasementFloor _BasementFloor => Instance.Basement;
    public static RoomsFloor[] _RoomFloors => Instance.RoomFloors;
    public static Floor[] _Floors => Instance.Floors;
    public static Lift[] _Lifts => Instance.Lifts;

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
            else if(floor is BasementFloor)
            {
                Basement = floor as BasementFloor;
            }
        }

        StartCoroutine(Initialize());
    }

    public static void CheckClean()
    {
        int cleanFloors = 0;
        foreach(RoomsFloor floor in Instance.RoomFloors)
        {
            cleanFloors += floor._Clear.GetHashCode();
        }

        if (cleanFloors == Instance.RoomFloors.Length)
        {
            foreach(Lift lift in Instance.Lifts)
            {
                lift.SetControlPanel(1);
            }

            Instance.Manager.SetActive(true);

            Instance.Task.Info = $"Подняться на 8 этаж";
        }
        else
        {
            foreach (Lift lift in Instance.Lifts)
            {
                lift.SetControlPanel(0);
            }

            Instance.Task.Info = $"{LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable", "CleanNumbersText")} <b>{cleanFloors}/{Instance.RoomFloors.Length}</b>";
            Instance.Manager.SetActive(false);

            Instance.Task.Info = $"Убрать номера во всех этажах <b>{cleanFloors}/{Instance.RoomFloors.Length}</b>";
        }
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        CheckClean();
        TaskDisplayer.ApplyTask(Task, false);
    }
}
