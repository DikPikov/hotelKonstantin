
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public interface IInteractable
{
    public void Interact();
    public float _BeforeTime { get; }
    public bool _CanInteract { get; }
}

public interface IAnimated : IInteractable
{
    public void Animate(bool state);
}

public class Interactable : MonoBehaviour, IInteractable
{
    [System.Serializable] public class MyInteractEvent : UnityEvent { }

    [FormerlySerializedAs("onInteract")]
    [SerializeField]
    protected MyInteractEvent OnInteract = new MyInteractEvent();

    [SerializeField] private float BeforeTime;
    [SerializeField] private bool CanInteract = true;

    public float _BeforeTime => BeforeTime;
    public bool _CanInteract
    {
        get
        {
            return CanInteract;
        }
        set
        {
            CanInteract = value;
        }
    }

    public void Interact()
    {
        OnInteract.Invoke();
    }
}