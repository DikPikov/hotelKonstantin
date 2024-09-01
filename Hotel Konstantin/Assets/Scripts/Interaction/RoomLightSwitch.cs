using UnityEngine;

public class RoomLightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Room Room;
    [SerializeField] private GameObject Light;
    [SerializeField] private Animator Animator;
    [SerializeField] private bool Enabled;

    public float _BeforeTime => 0.1f;
    public bool _CanInteract => true;
    public bool _Enabled
    {
        get
        {
            return Enabled;
        }
        set
        {
            Enabled = value;

            Animator.SetBool("On", Enabled);
            Light.SetActive(Enabled);

            Room.UpdateTaskInfo();
        }
    }

    public void Interact()
    {
        _Enabled = !Enabled;
    }
}
