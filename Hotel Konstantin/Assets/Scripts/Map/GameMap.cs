using UnityEditor.Localization.Reporting;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    private static GameMap Instance;

    [SerializeField] private TaskInfo Task;
    [SerializeField] private TaskDisplayer TaskDisplayer;

    [SerializeField] private Floor[] Floors;

    public static Floor[] _Floors => Instance.Floors;

    private void Awake()
    {
        Instance = this;

        TaskDisplayer.ApplyTask(Task, false);
    }

    public static void CheckClean()
    {
        int cleanFloors = 0;
        foreach(Floor floor in Instance.Floors)
        {
            cleanFloors += floor._Clear.GetHashCode();
        }

        Instance.Task.Info = $"Убрать номера во всех этажах <b>{cleanFloors}/{Instance.Floors.Length}</b>\nИли\nИзбавиться от монстров в отеле";
    }
}
