using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable]
    public class MouseEvent : UnityEvent<PointerEventData> { }

    [SerializeField] private MouseEvent OnMouseDown;
    [SerializeField] private MouseEvent OnMouseUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnMouseUp.Invoke(eventData);
    }
}
