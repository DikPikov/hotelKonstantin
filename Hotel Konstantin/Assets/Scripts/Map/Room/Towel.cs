using UnityEngine;

public class Towel : MonoBehaviour, IInteractable
{
    [SerializeField] private Room Room;
    [SerializeField] private Animator Animator;

    [SerializeField] private AudioSource Sound;

    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private Material[] Materials;

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

            if (Updated)
            {
                Sound.Play();
                Animator.Play("update");
            }

            Room.UpdateTaskInfo();
        }
    }

    public void SetDirtMaterial()
    {
        Renderer.material = Materials[1];
    }

    public void SetCleanMaterial()
    {
        Renderer.material = Materials[0];
    }

    public bool _CanInteract => !Updated;

    public void Interact()
    {
        _Updated = !Updated;
    }
}
