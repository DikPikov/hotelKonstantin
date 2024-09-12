using UnityEngine;

public class Comod : MonoBehaviour, OpenCloseDetector
{
    [SerializeField] private Room Room;
    [SerializeField] private SpawnPointInfo[] SpawnPoints;

    [SerializeField] private OpenClose[] OpenCloses;

    public SpawnPointInfo[] _SpawnPoints => SpawnPoints;
    public bool _Closed
    {
        get
        {
            foreach (OpenClose openClose in OpenCloses)
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