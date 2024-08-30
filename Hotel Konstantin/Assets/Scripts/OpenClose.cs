using UnityEngine;

public class OpenClose : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator Animator;
    [SerializeField] private float OpenTime;
    [SerializeField] private bool Opened;

    public float _BeforeTime => OpenTime;

    private void Start()
    {
        Animator.SetBool("isOpen", Opened);
    }

    public void Interact()
    {
        Opened = !Opened;

        Animator.SetBool("isOpen", Opened);
    }
}
