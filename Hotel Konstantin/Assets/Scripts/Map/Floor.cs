using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Floor : MonoBehaviour
{
    [SerializeField] private GameObject GhostPrefab;

    [SerializeField] private TaskDisplayer TaskDisplayer;
    [SerializeField] private TaskInfo TaskInfo;
    [SerializeField] private Player Player;

    [SerializeField] private Room[] Rooms;
    [SerializeField] private CoridorLights CoridorLights;
    [SerializeField] private Image GhostNoise;

    private Ghost Ghost = null;

    private void Start()
    {
        UpdateTaskInfo();

        CoridorLights._LightTime = Random.Range(60, 180f - 90 * Game._HotelMadness);
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
            if(Ghost != null)
            {
                Destroy(Ghost.gameObject);
            }
        }
        else if(Ghost == null)
        {
            Ghost = Instantiate(GhostPrefab, GetSpawnPoint(15), transform.rotation).GetComponent<Ghost>();
            Ghost.SetInfo(Player, CoridorLights, GhostNoise);
        }
    }

    public Vector3 GetSpawnPoint(float distance)
    {
        Vector3 vector3 = new Vector3() ;

        int times = 10;
        while (times > 0)
        {
            vector3 = new Vector3(Random.Range(82, -13), 1 + transform.position.y, Random.Range(0, 15));

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(vector3, Player.transform.position, -1, path);
            times--;

            if(path.status != NavMeshPathStatus.PathInvalid && Vector3.Distance(Player.transform.position, vector3) > distance)
            {
                break;
            }
        }

        return vector3;
    }

    public void UpdateTaskInfo()
    {
        int roomClear = 0;
        foreach(Room room in Rooms)
        {
            roomClear += room._Clear.GetHashCode();
        }

        TaskInfo.Info = $"Навести порядок в номерах <b>{roomClear}/{Rooms.Length}</b>";

        TaskDisplayer.UpdateInfo();
    }

    public void ShowTask(bool state) => TaskDisplayer.ApplyTask(TaskInfo, !state);
}
