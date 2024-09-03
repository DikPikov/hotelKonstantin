using UnityEngine;

public class Player : MonoBehaviour, ILiftable
{
    [SerializeField] private Floor Floor;

    [SerializeField] private float Sanity;
    [SerializeField] private float Stamina;

    private ItemObject CurrentItem = null;
    private Item[] Items = new Item[0];

    public event SimpleVoid OnChanges = null;
    public event SimpleVoid OnItemChanges = null;

    public Transform _Transform => transform;
    public ItemObject _CurrentItem
    {
        get
        {
            return CurrentItem;
        }
        set
        {
            if(CurrentItem != null && CurrentItem != value)
            {
                Destroy(CurrentItem.gameObject);
            }

            CurrentItem = value;
        }
    }
    public Item[] _Items => Items;

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

    public float _Sanity
    {
        get
        {
            return Sanity;
        }
        set
        {
            Sanity = Mathf.Clamp01(value);

            if(OnChanges != null)
            {
                OnChanges.Invoke();
            }
        }
    }

    public float _Stamina
    {
        get
        {
            return Stamina;
        }
        set
        {
            Stamina = Mathf.Clamp(value, 0, 5);

            if (OnChanges != null)
            {
                OnChanges.Invoke();
            }
        }
    }

    public void ApplyItem(Item item, bool remove)
    {
        if (remove)
        {
            Items = StaticTools.RemoveFromMassive(Items, item);
        }
        else if(Items.Length < 3)
        {
            Items = StaticTools.ExcludingExpandMassive(Items, item);
        }

        if(OnItemChanges != null)
        {
            OnItemChanges.Invoke();
        }
    }
}
