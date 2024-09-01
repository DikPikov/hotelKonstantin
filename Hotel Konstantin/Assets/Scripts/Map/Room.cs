using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private TaskDisplayer TaskDisplayer;
    [SerializeField] private TaskInfo TaskInfo;
    [SerializeField] private Floor Floor;

    [SerializeField] private GameObject Light;
    [SerializeField] private Bed Bed;
    [SerializeField] private Towel Towel;
    [SerializeField] private OpenClose[] Cabinets;
    [SerializeField] private GameObject[] Trash;
    [SerializeField] private GameObject[] Dirt;

    [SerializeField] private bool BedCleared;
    [SerializeField] private bool TrashRemoved;
    [SerializeField] private bool TowelUpdated;
    [SerializeField] private bool CabinetsClosed;
    [SerializeField] private bool DirtWashed;
    [SerializeField] private bool LightsOff;

    public bool _Clear => BedCleared && TrashRemoved && TowelUpdated && CabinetsClosed && DirtWashed && LightsOff;

    private void Start()
    {
        UpdateTaskInfo();
    }

    public void DeleteTrash(GameObject trash)
    {
        Trash = StaticTools.RemoveFromMassive(Trash, trash);
    }
    public void DeleteDirt(GameObject dirt)
    {
        Dirt = StaticTools.RemoveFromMassive(Dirt, dirt);
    }

    public void UpdateTaskInfo()
    {
        LightsOff = !Light.activeSelf;

        BedCleared = Bed._Cleared;

        TrashRemoved = Trash.Length == 0;

        DirtWashed = Dirt.Length == 0;

        CabinetsClosed = true;
        foreach(OpenClose openClose in Cabinets)
        {
            if (openClose._Opened)
            {
                CabinetsClosed = false;
                break;
            }
        }

        TowelUpdated = Towel._Updated;

        TaskInfo.Info = $"Кровать заправлена <b>{(BedCleared ? "Да" : "Нет")}</b>\nМусор убран <b>{(TrashRemoved ? "Да" : "Нет")}</b>\nПолотенце обновлено <b>{(TowelUpdated ? "Да" : "Нет")}</b>" +
            $"\nЯщики, шкафы и двери закрыты <b>{(CabinetsClosed ? "Да" : "Нет")}</b>\nГрязь отсутствует <b>{(DirtWashed ? "Да" : "Нет")}</b>\nСвет выключен <b>{(LightsOff ? "Да" : "Нет")}</b>";
       
        Floor.UpdateTaskInfo(); 
        TaskDisplayer.UpdateInfo();
    }

    public void ShowTask(bool state) => TaskDisplayer.ApplyTask(TaskInfo, !state);
}
