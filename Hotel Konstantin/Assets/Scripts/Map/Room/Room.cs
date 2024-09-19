using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

public class Room : MonoBehaviour
{
    [SerializeField] private TaskDisplayer TaskDisplayer;
    [SerializeField] private TaskInfo TaskInfo;
    [SerializeField] private RoomsFloor Floor;

    [SerializeField] private GameObject TrashPrefab;
    [SerializeField] private Vector3[] TrashSpawnRect;
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private RoomPattern[] Presets;

    [SerializeField] private RoomLightSwitch LightSwitch = null;
    [SerializeField] private Lighter Lighter;
    [SerializeField] private GameObject[] Lustras;

    private Bed[] Bed = new Bed[0];
    private Towel Towel = null;
    private Televizor Televizor = null;
    private ComodLamp ComodLamp = null;
    private Shkaf Shkaf = null;
    private Comod Comod = null;

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

    public Shkaf _Shkaf => Shkaf;
    public Comod _Comod => Comod;

    public RoomLightSwitch _RoomLight => LightSwitch;
    public bool _Clear => BedCleared && TrashRemoved && TowelUpdated && CabinetsClosed && DirtWashed && LightsOff && TVOff;

    [SerializeField] private string[] localizedStrings;

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
    public void SetShkaf(Shkaf shkaf)
    {
        Shkaf = shkaf;
        Shkaf.Initialize();
    }
    public void SetComod(Comod comod)
    {
        Comod = comod;
        Comod.Initialize();
    }

    private void Start()
    {
        int preset = Random.Range(0, Presets.Length);

        for(int i = 0; i < Presets.Length; i++)
        {
            if(i != preset)
            {
                Destroy(Presets[i].gameObject);
            }
        }

        Presets[preset].GiveReferences();

        LightSwitch._Enabled = Random.value > 0.7f;

        int lustra = Random.Range(0, Lustras.Length);
        for (int i = 0; i < Lustras.Length; i++)
        {
            if (i != lustra)
            {
                Destroy(Lustras[i]);
            }
        }

        if(lustra == 2)
        {
            Lighter._LightMaterialIndex = 1;
        }
        Lighter._Renderer = Lustras[lustra].GetComponent<MeshRenderer>();

        StartCoroutine(SpawnOther());

        Floor.AddRoom(this);

        localizedStrings[0] = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","YesText");
        localizedStrings[1] = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","NoText");
        localizedStrings[2] = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","BedTaskText");
        localizedStrings[3] = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","TrashTaskText");
        localizedStrings[4] = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","TowelTaskText");
        localizedStrings[5] = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","WardrobeDoorsTaskText");
        localizedStrings[6] = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","DirtTaskText");
        localizedStrings[7] = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","LightTaskText");
        localizedStrings[8] = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","TvTaskText");

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

        CabinetsClosed = Shkaf._Closed && Comod._Closed;

        TowelUpdated = Towel._Updated;

        TaskInfo.Info = $"{localizedStrings[2]} : <b>{(BedCleared ? localizedStrings[0] : localizedStrings[1])}</b>\n{localizedStrings[3]} : <b>{(TrashRemoved ? localizedStrings[0] : localizedStrings[1])}</b>\n{localizedStrings[4]} : <b>{(TowelUpdated ? localizedStrings[0] : localizedStrings[1])}</b>" +
            $"\n{localizedStrings[5]} : <b>{(CabinetsClosed ? localizedStrings[0] : localizedStrings[1])}</b>\n{localizedStrings[6]} : <b>{(DirtWashed ? localizedStrings[0] : localizedStrings[1])}</b>\n{localizedStrings[7]} : <b>{(LightsOff ? localizedStrings[0] : localizedStrings[1])}</b>\n{localizedStrings[8]} : <b>{(TVOff ? localizedStrings[0] : localizedStrings[1])}</b>";
       
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

    public Vector3 GetSpawnPoint() => new Vector3(Random.Range(TrashSpawnRect[0].x, TrashSpawnRect[1].x), 0, Random.Range(TrashSpawnRect[0].z, TrashSpawnRect[1].z)) + transform.position;

    private IEnumerator SpawnOther()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        //Debug.DrawRay(transform.position + TrashSpawnRect[0], Vector2.up * 5, Color.red);
        //Debug.DrawRay(transform.position + TrashSpawnRect[1], Vector2.up * 5, Color.green);

        int count = Random.Range(0, 5);
        Trash = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            Transform trash = Instantiate(TrashPrefab, transform).transform;
            trash.localScale = new Vector3(Random.Range(14f, 16f), Random.Range(14f, 16), Random.Range(14f, 16));
            trash.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);

            Vector3 position = GetSpawnPoint() - transform.position + Vector3.up * 2.7f;

            RaycastHit hit;
            if (Physics.Raycast(position + transform.position, Vector3.down, out hit, 10, LayerMask))
            {
                trash.transform.position = hit.point + new Vector3(0, 0.17f, 0);
            }
            else
            {
                position.y = 1.17f;
                trash.transform.localPosition = position;
            }

            trash.GetComponent<Trash>().SetRoom(this);
            Trash[i] = trash.gameObject;
        }
    }
}
