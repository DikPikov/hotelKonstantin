using UnityEngine;

public class RoomPattern : MonoBehaviour
{
    [SerializeField] private Room Room;

    [SerializeField] private Bed[] Beds;
    [SerializeField] private Towel Towel;
    [SerializeField] private Televizor Televizor;
    [SerializeField] private ComodLamp ComodLamp;
    [SerializeField] private Shkaf Shkaf;
    [SerializeField] private Comod Comod;

    public void GiveReferences()
    {
        foreach(Bed bed in Beds)
        {
            Room.AddBed(bed);
        }

        Room.SetTowel(Towel);
        Room.SetTV(Televizor);
        Room.SetCommodLamp(ComodLamp);
        Room.SetShkaf(Shkaf);
        Room.SetComod(Comod);
    }
}
