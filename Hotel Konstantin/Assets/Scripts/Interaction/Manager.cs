using UnityEngine;

public class Manager : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform Player;
    [SerializeField] private Animator Animator;

    public float _BeforeTime => 1;
    public bool _CanInteract => true;

    private void Update()
    {
        transform.localEulerAngles = new Vector3(0, (Quaternion.LookRotation(Player.position - transform.position).eulerAngles).y, 0 );
    }

    public void Interact()
    {
        Animator.SetBool("givemoney", true);
    }
}
