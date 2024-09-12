using UnityEngine;
using UnityEngine.Rendering;

public class Shkaf : MonoBehaviour, OpenCloseDetector
{
    [SerializeField] private Room Room;
    [SerializeField] private SpawnPointInfo[] RomSpawns;
    [SerializeField] private SpawnPointInfo[] WinchesterSpawns;

    [SerializeField] private OpenClose[] OpenCloses;

    public SpawnPointInfo[] _RomSpawns => RomSpawns;
    public SpawnPointInfo[] _WinchesterSpawns => WinchesterSpawns;
    public bool _Closed
    {
        get
        {
            foreach(OpenClose openClose in OpenCloses)
            {
                if (openClose._Opened)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public void Initialize()
    {
        foreach (OpenClose openClose in OpenCloses)
        {
            openClose._Detector = this;
            openClose.enabled = Random.value > 0.85f;
        }
    }

    public void OpenStateUpdate()
    {
        Room.UpdateTaskInfo();
    }
}
