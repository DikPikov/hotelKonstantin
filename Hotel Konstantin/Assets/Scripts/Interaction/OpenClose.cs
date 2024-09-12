using UnityEngine;

public interface OpenCloseDetector
{
    public void OpenStateUpdate();
}

public class OpenClose : MonoBehaviour, IInteractable
{
    protected OpenCloseDetector Detector;

    [SerializeField] protected Animator Animator;
    [SerializeField] protected float OpenTime;
    [SerializeField] protected bool Opened;

    public OpenCloseDetector _Detector
    {
        get
        {
            return Detector;
        }
        set
        {
            Detector = value;
        }
    }
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

        if(Detector != null)
        {
            Detector.OpenStateUpdate();
        }
    }
}
