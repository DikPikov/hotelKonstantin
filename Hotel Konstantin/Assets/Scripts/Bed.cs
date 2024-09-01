using UnityEngine;

public class Bed : OpenClose
{
    [SerializeField] private Room Room;

    public override void Interact()
    {
        base.Interact();

        Room.UpdateTaskInfo();
    }
}
