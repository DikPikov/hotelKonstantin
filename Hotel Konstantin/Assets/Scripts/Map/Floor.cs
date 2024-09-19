using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Floor : MonoBehaviour
{

    [SerializeField] protected TaskDisplayer TaskDisplayer;
    [SerializeField] protected TaskInfo TaskInfo;
    [SerializeField] protected Player Player;

    [SerializeField] protected int Index;

    [SerializeField] protected Vector3[] CoridorBorders;

    public Player _Player => Player;
    public int _Index => Index;

    public virtual Vector3 GetSpawnPoint()
    {
        return new Vector3(Random.Range(CoridorBorders[0].x, CoridorBorders[1].x), transform.position.y + 1, Random.Range(CoridorBorders[0].z, CoridorBorders[1].z));
    }

    public void LocalizeText(string localizedText)
    {
        TaskInfo.Info = localizedText;
    }

    public void ShowTask(bool state) => TaskDisplayer.ApplyTask(TaskInfo, !state);
}
