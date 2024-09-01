using UnityEngine;

public class Towel : MonoBehaviour, IInteractable
{
    [SerializeField] private Room Room;
    [SerializeField] private Animator Animator;
    [SerializeField] private bool Updated;

    public float _BeforeTime => 2;
    public bool _Updated
    {
        get
        {
            return Updated;
        }
        set
        {
            Updated = value;

            Animator.SetBool("Updated", Updated);

            Room.UpdateTaskInfo();
        }
    }
    public bool _CanInteract => !Updated;

    public void Interact()
    {
        _Updated = !Updated;
    }
}
