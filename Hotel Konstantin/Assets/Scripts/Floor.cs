using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private GameObject GhostPrefab;

    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private TaskDisplayer TaskDisplayer;
    [SerializeField] private TaskInfo TaskInfo;
    [SerializeField] private Player Player;

    [SerializeField] private Room[] Rooms;
    [SerializeField] private CoridorLights CoridorLights;
    private Ghost Ghost = null;


    private void Start()
    {
        UpdateTaskInfo();
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
            Ghost = Instantiate(GhostPrefab, SpawnPoint.position, transform.rotation).GetComponent<Ghost>();
            Ghost.SetInfo(Player, CoridorLights);
        }
    }

    public void UpdateTaskInfo()
    {
        int roomClear = 0;
        foreach(Room room in Rooms)
        {
            roomClear += room._Clear.GetHashCode();
        }

        TaskInfo.Info = $"Навести порядок в номерах <b>{roomClear}/{Rooms.Length}</b>";
    }

    public void ShowTask(bool state) => TaskDisplayer.ApplyTask(TaskInfo, !state);
}
