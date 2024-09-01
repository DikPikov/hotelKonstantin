using UnityEngine;

public class Door : OpenClose
{
    [SerializeField] private OcclusionPortal Portal;

    public override void Interact()
    {
        base.Interact();

        Portal.open = Opened;
    }
}
