using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform[] InteractPoints;

    [SerializeField] private Animator Animator;
    [SerializeField] private AudioSource[] OpenSounds;
    [SerializeField] private AudioSource[] CloseSounds;
    [SerializeField] private bool Opened;

    public Transform[] _Points => InteractPoints;
    public float _BeforeTime => 0.3f;
    public bool _CanInteract => true;

    public bool _Opened
    {
        get
        {
            return Opened;
        }
        set
        {
            Opened = value;

            Animator.SetBool("isOpen", value);

            if (Opened)
            {
                OpenSounds[Random.Range(0, OpenSounds.Length)].Play();
            }
            else 
            {
                CloseSounds[Random.Range(0, CloseSounds.Length)].Play();
            }
        }
    }

    public void SetBarriering(bool state) => Animator.SetBool("barriering", state);

    public void PlayBreaking()
    {
        Animator.Play("break");
    }

    public void Interact() => _Opened = !Opened;
}
