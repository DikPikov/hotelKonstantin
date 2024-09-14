using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System.Collections;

public class RoomsFloor : Floor
{
    [SerializeField] private GameObject GhostPrefab;
    [SerializeField] private Image GhostNoise;
    private Ghost Ghost = null;

    [SerializeField] private Room[] Rooms;
    [SerializeField] private CoridorLights CoridorLights;

    private bool Clear = false;

    public Room[] _Rooms => Rooms;
    public CoridorLights _Light => CoridorLights;
    public bool _Clear => Clear;

    protected override void Start()
    {

        StartCoroutine(Initialize());
        //Debug.DrawRay(transform.position + FloorBorders[0], Vector3.up * 10, Color.red);
        //Debug.DrawRay(transform.position + FloorBorders[1], Vector3.up * 10, Color.red);
    }

    public void AddRoom(Room room)
    {
        Rooms = StaticTools.ExpandMassive(Rooms, room);
        CoridorLights.AddLighter(room._RoomLight);
    }

    public void SpawnGhost(bool despawn)
    {
        if (despawn)
        {
            if (Ghost != null)
            {
                Destroy(Ghost.gameObject);
            }
        }
        else if (Ghost == null)
        {
            Ghost = Instantiate(GhostPrefab, GetSpawnPoint(), transform.rotation).GetComponent<Ghost>();
            Ghost.SetInfo(Player, this, GhostNoise);
        }
    }

    public override Vector3 GetSpawnPoint()
    {
        int random = Random.Range(0, Rooms.Length + 4);
        if(random >= Rooms.Length)
        {
            return new Vector3(Random.Range(CoridorBorders[0].x, CoridorBorders[1].x), transform.position.y + 1, Random.Range(CoridorBorders[0].z, CoridorBorders[1].z));
        }
        else
        {
            return Rooms[random].GetSpawnPoint() + Vector3.up;
        }

    }

    public void UpdateTaskInfo()
    {
        int roomClear = 0;
        foreach(Room room in Rooms)
        {
            roomClear += room._Clear.GetHashCode();
        }

        bool clear = roomClear == Rooms.Length;
        if(clear != Clear)
        {
            Clear = clear;
            GameMap.CheckClean();
        }

        TaskInfo.Info = $"{LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable","CorridorTaskText")} <b>{roomClear}/{Rooms.Length}</b>";

        TaskDisplayer.UpdateInfo();
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        UpdateTaskInfo();
    }
}
