using UnityEngine;

public class test : MonoBehaviour, IInteractable
{
    [SerializeField] private string Message;
    [SerializeField] private float Time;

    public float _BeforeTime => Time;
    public bool _CanInteract => true;

    public void Interact()
    {
        Debug.Log(Message);
    }
}
