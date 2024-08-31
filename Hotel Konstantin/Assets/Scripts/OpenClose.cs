using UnityEngine;

public class OpenClose : MonoBehaviour, IInteractable
{
    [SerializeField] protected Animator Animator;
    [SerializeField] protected float OpenTime;
    [SerializeField] protected bool Opened;

    public float _BeforeTime => OpenTime;

    private void Start()
    {
        Animator.SetBool("isOpen", Opened);
    }

    public virtual void Interact()
    {
        Opened = !Opened;

        Animator.SetBool("isOpen", Opened);
    }
}
