using UnityEngine;

public class ItemPick : MonoBehaviour, IInteractable, ILiftable
{
    [SerializeField] private string ItemType;
    private Item Item = null;

    private Floor Floor = null;

    public Transform _Transform => transform;
    public Floor _Floor
    {
        get
        {
            return Floor;
        }
        set
        {
            Floor = value;
        }
    }
    public float _BeforeTime => Item._PickTime;
    public bool _CanInteract => true;

    public void SetInfo(Item item)
    {
        Item = item;
        gameObject.name = item._Prefab;
    }

    private void Start()
    {
        if(ItemType != "" && Item == null)
        {
            Item = Item.Create(ItemType);
        }
    }

    public void Interact()
    {
        FindObjectOfType<Player>().ApplyItem(Item, false);
        Destroy(gameObject);
    }
}
