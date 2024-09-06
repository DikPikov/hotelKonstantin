using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private TaskDisplayer TaskDisplayer;
    [SerializeField] private TaskInfo TaskInfo;
    [SerializeField] private Floor Floor;

    [SerializeField] private GameObject TrashPrefab;
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private GameObject[] Presets;

    [SerializeField] private RoomLightSwitch LightSwitch = null;
    [SerializeField] private Lighter Lighter;
    [SerializeField] private GameObject[] Lustras;

    private Bed[] Bed = new Bed[0];
    private Towel Towel = null;
    private Televizor Televizor = null;
    private ComodLamp ComodLamp = null;
    private OpenClose[] Cabinets= new OpenClose[0];
     private GameObject[] Trash = new GameObject[0];
     private GameObject[] Dirt = new GameObject[0];

    private bool BedCleared;
    private bool TrashRemoved;
    private bool TowelUpdated;
    private bool CabinetsClosed;
    private bool DirtWashed;
    private bool LightsOff;
    private bool TVOff;

    private bool Initialized = false;

    public Lighter _RoomLight => LightSwitch._Lighter;
    public bool _Clear => BedCleared && TrashRemoved && TowelUpdated && CabinetsClosed && DirtWashed && LightsOff && TVOff;

    public void AddBed(Bed bed)
    {
        Bed = StaticTools.ExpandMassive(Bed, bed);
        bed._Cleared = Random.value > 0.75f;
    }
    public void SetTowel(Towel towel)
    {
        Towel = towel;
        Towel._Updated = Random.value > 0.6f;
    }
    public void SetTV(Televizor televizor)
    {
        Televizor = televizor;
        Televizor._On = Random.value > 0.8f;
    }
    public void SetCommodLamp(ComodLamp lamp)
    {
        ComodLamp = lamp;
        lamp._On = Random.value > 0.4f;
    }
    public void AddCabinet(OpenClose openClose)
    {
        Cabinets = StaticTools.ExpandMassive(Cabinets, openClose);
        openClose._Opened = Random.value > 0.85f;
    }

    private void Start()
    {
        Presets[Random.Range(0, Presets.Length)].SetActive(true);

        //int count = Random.Range(0, 5);
        //Trash = new GameObject[count];
        //for (int i = 0; i < count; i++)
        //{
        //    Transform trash = Instantiate(TrashPrefab, transform).transform;

        //    Vector3 position = new Vector3(Random.Range(5.5f, 10), 2.7f, Random.Range(-6.5f, 4.5f));

        //    RaycastHit hit;
        //    if(Physics.Raycast(position + transform.position, Vector3.down, out hit, 10, LayerMask))
        //    {
        //        trash.transform.position = hit.point + new Vector3(0, 0.17f, 0);
        //    }
        //    else
        //    {
        //        position.y = 1.17f;
        //        trash.transform.localPosition = position;
        //    }

        //    trash.GetComponent<Trash>().SetRoom(this);
        //    Trash[i] = trash.gameObject;
        //}

        LightSwitch._Enabled = Random.value > 0.7f;

        int lustra = Random.Range(0, Lustras.Length);
        Lustras[lustra].SetActive(true);
        if(lustra == 2)
        {
            Lighter._LightMaterialIndex = 1;
        }
        Lighter._Renderer = Lustras[lustra].GetComponent<MeshRenderer>();

        Floor.AddRoom(this);

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
        if (!Initialized)
        {
            return;
        }

        LightsOff = !LightSwitch._Enabled && !ComodLamp._On;

        TVOff = !Televizor._On;

        BedCleared = true;
        foreach(Bed bed in Bed)
        {
            if(!bed._Cleared)
            {
                BedCleared = false;
                break;
            }
        }

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

        TaskInfo.Info = $"Кровати заправлены <b>{(BedCleared ? "Да" : "Нет")}</b>\nМусор убран <b>{(TrashRemoved ? "Да" : "Нет")}</b>\nПолотенце обновлено <b>{(TowelUpdated ? "Да" : "Нет")}</b>" +
            $"\nЯщики, шкафы и двери закрыты <b>{(CabinetsClosed ? "Да" : "Нет")}</b>\nГрязь отсутствует <b>{(DirtWashed ? "Да" : "Нет")}</b>\nСвет выключен <b>{(LightsOff ? "Да" : "Нет")}</b>\nЭЛТ Телевизор выключен <b>{(TVOff ? "Да" : "Нет")}</b>";
       
        Floor.UpdateTaskInfo(); 
        TaskDisplayer.UpdateInfo();
    }

    public void ShowTask(bool state)
    {
        if (!Initialized) 
        {
            Initialized = true;
            UpdateTaskInfo();
        }

        TaskDisplayer.ApplyTask(TaskInfo, !state);
    }
}
