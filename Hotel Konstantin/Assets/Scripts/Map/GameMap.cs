using UnityEngine;

public class GameMap : MonoBehaviour
{
    private static GameMap Instance;

    [SerializeField] private Floor[] Floors;

    public static Floor[] _Floors => Instance.Floors;

    private void Awake()
    {
        Instance = this;
    }
}
