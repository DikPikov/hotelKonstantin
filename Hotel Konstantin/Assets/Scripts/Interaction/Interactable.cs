
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public interface IInteractable
{
    public void Interact();
    public float _BeforeTime { get; }
}

public class Interactable : MonoBehaviour, IInteractable
{
    [System.Serializable] public class MyInteractEvent : UnityEvent { }

    [FormerlySerializedAs("onInteract")]
    [SerializeField]
    protected MyInteractEvent OnInteract = new MyInteractEvent();

    [SerializeField] private float BeforeTime;

    public float _BeforeTime => BeforeTime;

    public void Interact()
    {
        OnInteract.Invoke();
    }
}