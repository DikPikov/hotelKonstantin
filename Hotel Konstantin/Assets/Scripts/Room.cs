using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private TaskDisplayer TaskDisplayer;
    [SerializeField] private TaskInfo TaskInfo;
    [SerializeField] private Floor Floor;

    [SerializeField] private OpenClose[] Cabinets;

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
        TaskInfo.Info = $"������� ���������� <b>{(BedCleared ? "��" : "���")}</b>\n����� ����� <b>{(TrashRemoved ? "��" : "���")}</b>\n��������� ��������� <b>{(TowelUpdated ? "��" : "���")}</b>" +
            $"\n�����, ����� � ����� ������� <b>{(CabinetsClosed ? "��" : "���")}</b>\n����� ����������� <b>{(DirtWashed ? "��" : "���")}</b>\n���� �������� <b>{(LightsOff ? "��" : "���")}</b>";
    }

    public void ShowTask(bool state) => TaskDisplayer.ApplyTask(TaskInfo, !state);
}
