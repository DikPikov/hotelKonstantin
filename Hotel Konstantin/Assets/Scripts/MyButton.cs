using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MyButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [System.Serializable] public class MyClickEvent : UnityEvent { }

    [FormerlySerializedAs("onClick")]
    [SerializeField]
    protected MyClickEvent m_OnClick = new MyClickEvent();

    [SerializeField] protected Image Image;
    [SerializeField] protected Color SelectColor;
    [SerializeField] protected Color DefaultColor;

    public Color _DefaultColor 
    { 
        get 
        { 
            return DefaultColor; 
        } 
        set 
        { 
            DefaultColor = value;

                Image.color = DefaultColor;
        } 
    }

    protected virtual void OnDisable()
    {
        Image.color = DefaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_OnClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Image.color = SelectColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image.color = DefaultColor;
    }
}
