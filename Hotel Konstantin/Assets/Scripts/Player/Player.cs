using UnityEngine;

public class Player : MonoBehaviour, ILiftable
{
    [SerializeField] private Floor Floor;

    [SerializeField] private float Stamina;

    private ItemObject CurrentItem = null;
    private Item[] Items = new Item[0];

    public event SimpleVoid OnFloorChange = null;
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

            if (OnItemChanges != null)
            {
                OnItemChanges.Invoke();
            }
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
            if(Floor == value)
            {
                return;
            }

            //if (Floor != null)
            //{
            //    Floor.gameObject.SetActive(false);
            //}

            Floor = value;

            //if (Floor != null)
            //{
            //    Floor.gameObject.SetActive(true);
            //}

            if (OnFloorChange != null)
            {
                OnFloorChange.Invoke();
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

    public bool ApplyItem(Item item, bool remove)
    {
        int index = StaticTools.IndexOf(Items, item);

        if (remove)
        {
            if(index < 0)
            {
                return false;
            }
            else
            {
                if(CurrentItem._Item == Items[index])
                {
                    _CurrentItem = null;
                }

                Items = StaticTools.ReduceMassive(Items, index);
            }
        }
        else
        {
            if(Items.Length >= 3)
            {
                return false;
            }

            if(index < 0)
            {
                Items = StaticTools.ExcludingExpandMassive(Items, item);
            }
            else
            {
                return false;
            }
        }

        if(OnItemChanges != null)
        {
            OnItemChanges.Invoke();
        }

        return true;
    }
}
