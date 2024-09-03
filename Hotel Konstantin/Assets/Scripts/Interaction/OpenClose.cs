using UnityEngine;

public class OpenClose : MonoBehaviour, IInteractable
{
    [SerializeField] protected Room Room;

    [SerializeField] protected Animator Animator;
    [SerializeField] protected float OpenTime;
    [SerializeField] protected bool Opened;

    public bool _Opened
    {
        get
        {
            return Opened;
        }
        set
        {
            Opened = !value;
            Interact();
        }
    }
    public  float _BeforeTime => OpenTime;
    public virtual bool _CanInteract => true;

    private void Start()
    {
        Animator.SetBool("isOpen", Opened);
    }

    public virtual void Interact()
    {
        Opened = !Opened;

        Animator.SetBool("isOpen", Opened);

        if(Room != null)
        {
            Room.UpdateTaskInfo();
        }
    }
}
