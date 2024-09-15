using UnityEngine;

public class BasementFloor : Floor
{
    [SerializeField] private FuseSwitch[] FloorsFuses;
     
    public FuseSwitch[] _Fuses => FloorsFuses;

    public void InitializeFuses()
    {
        FloorsFuses[0].SetFuseNoNotify(new DamagedFuse());

        int[] used = new int[] { 0 };
        for (int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, FloorsFuses.Length);
            while (StaticTools.IndexOf(used, random) > -1)
            {
                random = Random.Range(0, FloorsFuses.Length);
            }

            used = StaticTools.ExpandMassive(used, random);
            FloorsFuses[random].SetFuseNoNotify(new DamagedFuse());
        }

        for (int i = 0; i < GameMap._RoomFloors.Length; i++)
        {
            if (!StaticTools.Contains(used, i))
            {
                FloorsFuses[i]._Fuse = null;
            }
        }
    }
}
