using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Trigger : MonoBehaviour
{
    [System.Serializable] public class MyTriggerEvent : UnityEvent { }

    [FormerlySerializedAs("onEnter")]
    [SerializeField]
    protected MyTriggerEvent OnEnter = new MyTriggerEvent();

    [FormerlySerializedAs("onEnter")]
    [SerializeField]
    protected MyTriggerEvent OnExit = new MyTriggerEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Player>())
        {
            OnEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<Player>())
        {
            OnExit.Invoke();
        }
    }
}
