using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemManager ItemManager;
    [SerializeField] private int WinchesterCount;
    [SerializeField] private int FuseCount;
    [SerializeField] private int PatronCount;
    [SerializeField] private int RomCount;

    private void Start()
    {
        StartCoroutine(SpawnItems());
    }

    public IEnumerator SpawnItems()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Shkaf[] shkafs = new Shkaf[WinchesterCount + RomCount];
        int index = 0;

        for (int i = 0; i < WinchesterCount; i++)
        {
            int randomFloor = Random.Range(1, GameMap._Floors.Length);
            int randomRoom = Random.Range(0, GameMap._Floors[randomFloor]._Rooms.Length);

            Shkaf shkaf1 = GameMap._Floors[randomFloor]._Rooms[randomRoom]._Shkaf;

            while (StaticTools.Contains(shkafs, shkaf1))
            {
                randomFloor = Random.Range(1, GameMap._Floors.Length);
                randomRoom = Random.Range(0, GameMap._Floors[randomFloor]._Rooms.Length);

                shkaf1 = GameMap._Floors[randomFloor]._Rooms[randomRoom]._Shkaf;
            }

            Transform item = ItemManager.SpawnItem(new Winchester()).transform;

            int randomPoint = Random.Range(0, shkaf1._WinchesterSpawns.Length);
            item.parent = shkaf1._WinchesterSpawns[randomPoint].Parent;
            item.localPosition = shkaf1._WinchesterSpawns[randomPoint].Position;
            item.localEulerAngles = shkaf1._WinchesterSpawns[randomPoint].Rotation;
            item.GetChild(0).localEulerAngles = Vector3.zero;

            shkafs[index] = shkaf1;
            index++;
        }

        for (int i = 0; i < RomCount; i++)
        {
            int randomFloor = Random.Range(1, GameMap._Floors.Length);
            int randomRoom = Random.Range(0, GameMap._Floors[randomFloor]._Rooms.Length);

            Shkaf shkaf1 = GameMap._Floors[randomFloor]._Rooms[randomRoom]._Shkaf;

            while (StaticTools.Contains(shkafs, shkaf1))
            {
                randomFloor = Random.Range(1, GameMap._Floors.Length);
                randomRoom = Random.Range(0, GameMap._Floors[randomFloor]._Rooms.Length);

                shkaf1 = GameMap._Floors[randomFloor]._Rooms[randomRoom]._Shkaf;
            }

            Transform item = ItemManager.SpawnItem(new Alcohol()).transform;

            int randomPoint = Random.Range(0, shkaf1._RomSpawns.Length);
            item.parent = shkaf1._RomSpawns[randomPoint].Parent;
            item.localPosition = shkaf1._RomSpawns[randomPoint].Position;
            item.localEulerAngles = shkaf1._RomSpawns[randomPoint].Rotation;

            shkafs[index] = shkaf1;
            index++;
        }

        Comod[] comods = new Comod[PatronCount + FuseCount];
        index = 0;

        for (int i = 0; i < FuseCount; i++)
        {
            int randomFloor = Random.Range(1, GameMap._Floors.Length);
            int randomRoom = Random.Range(0, GameMap._Floors[randomFloor]._Rooms.Length);

            Comod comod = GameMap._Floors[randomFloor]._Rooms[randomRoom]._Comod;

            while (StaticTools.Contains(comods, comod))
            {
                randomFloor = Random.Range(1, GameMap._Floors.Length);
                randomRoom = Random.Range(0, GameMap._Floors[randomFloor]._Rooms.Length);

                comod = GameMap._Floors[randomFloor]._Rooms[randomRoom]._Comod;
            }

            Transform item = ItemManager.SpawnItem(new StableFuse()).transform;

            int randomPoint = Random.Range(0, comod._SpawnPoints.Length);
            item.parent = comod._SpawnPoints[randomPoint].Parent;
            item.localPosition = comod._SpawnPoints[randomPoint].Position;
            item.localEulerAngles = comod._SpawnPoints[randomPoint].Rotation;

            comods[index] = comod;
            index++;
        }

        for (int i = 0; i < PatronCount; i++)
        {
            int randomFloor = Random.Range(1, GameMap._Floors.Length);
            int randomRoom = Random.Range(0, GameMap._Floors[randomFloor]._Rooms.Length);

            Comod comod = GameMap._Floors[randomFloor]._Rooms[randomRoom]._Comod;

            while (StaticTools.Contains(comods, comod))
            {
                randomFloor = Random.Range(1, GameMap._Floors.Length);
                randomRoom = Random.Range(0, GameMap._Floors[randomFloor]._Rooms.Length);

                comod = GameMap._Floors[randomFloor]._Rooms[randomRoom]._Comod;
            }

            Transform item = ItemManager.SpawnItem(new Bullet()).transform;

            int randomPoint = Random.Range(0, comod._SpawnPoints.Length);
            item.parent = comod._SpawnPoints[randomPoint].Parent;
            item.localPosition = comod._SpawnPoints[randomPoint].Position;
            item.localEulerAngles = comod._SpawnPoints[randomPoint].Rotation;

            comods[index] = comod;
            index++;
        }
    }
}
