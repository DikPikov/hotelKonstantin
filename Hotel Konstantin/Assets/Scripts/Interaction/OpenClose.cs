using UnityEngine;

public interface OpenCloseDetector
{
    public void OpenStateUpdate();
}

public class OpenClose : MonoBehaviour, IInteractable
{
    protected OpenCloseDetector Detector;

    [SerializeField] protected AudioSource[] OpenSound;
    [SerializeField] protected AudioSource[] CloseSound;
    [SerializeField] protected bool PlaySoundOnState;

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

    public virtual void PlayOpenSound()
    {
        int sound = Random.Range(0, OpenSound.Length);

        OpenSound[sound].pitch = Random.Range(0.9f, 1.1f);
        OpenSound[sound].Play();
    }
    public virtual void PlayCloseSound()
    {
        int sound = Random.Range(0, CloseSound.Length);

        CloseSound[sound].pitch = Random.Range(0.9f, 1.1f);
        CloseSound[sound].Play();
    }

    public virtual void Interact()
    {
        Opened = !Opened;

        if (PlaySoundOnState)
        {
            if (Opened)
            {
                PlayOpenSound();
            }
            else
            {
                PlayCloseSound();
            }
        }

        Animator.SetBool("isOpen", Opened);

        if(Detector != null)
        {
            Detector.OpenStateUpdate();
        }
    }
}
