using UnityEngine;

public class Towel : MonoBehaviour, IInteractable
{
    [SerializeField] private Room Room;
    [SerializeField] private Animator Animator;
    [SerializeField] private Collider Collider;
    [SerializeField] private bool Updated;

    public bool _Updated => Updated;
    public float _BeforeTime => 2;

    public void Interact()
    {
        Updated = true;
        Collider.enabled = false;

        Animator.SetBool("Updated", true);
    }
}
