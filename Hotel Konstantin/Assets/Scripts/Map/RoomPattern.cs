using UnityEngine;

public class RoomPattern : MonoBehaviour
{
    [SerializeField] private Room Room;

    [SerializeField] private Bed[] Beds;
    [SerializeField] private Towel Towel;
    [SerializeField] private Televizor Televizor;
    [SerializeField] private ComodLamp ComodLamp;
    [SerializeField] private OpenClose[] Cabinets;

    public void GiveReferences()
    {
        foreach(Bed bed in Beds)
        {
            Room.AddBed(bed);
        }

        Room.SetTowel(Towel);
        Room.SetTV(Televizor);
        Room.SetCommodLamp(ComodLamp);

        foreach(OpenClose openClose in Cabinets)
        {
            Room.AddCabinet(openClose);
        }
    }
}
