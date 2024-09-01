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

    public void UpdateTaskInfo()
    {
        LightsOff = !Light.activeSelf;

        BedCleared = Bed._Opened;

        TrashRemoved = true;
        foreach(GameObject trash in Trash)
        {
            if (trash.activeSelf)
            {
                TrashRemoved = false;
                break;
            }
        }

        DirtWashed = true;
        foreach (GameObject dirt in Dirt)
        {
            if (dirt.activeSelf)
            {
                DirtWashed = false;
                break;
            }
        }

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

        TaskInfo.Info = $"������� ���������� <b>{(BedCleared ? "��" : "���")}</b>\n����� ����� <b>{(TrashRemoved ? "��" : "���")}</b>\n��������� ��������� <b>{(TowelUpdated ? "��" : "���")}</b>" +
            $"\n�����, ����� � ����� ������� <b>{(CabinetsClosed ? "��" : "���")}</b>\n����� ����������� <b>{(DirtWashed ? "��" : "���")}</b>\n���� �������� <b>{(LightsOff ? "��" : "���")}</b>";
       
        Floor.UpdateTaskInfo(); 
        TaskDisplayer.UpdateInfo();
    }

    public void ShowTask(bool state) => TaskDisplayer.ApplyTask(TaskInfo, !state);
}
