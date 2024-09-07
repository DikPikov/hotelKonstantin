using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] protected Animator Animator;
    [SerializeField] protected Player Player;
    protected Item Item;

    public Item _Item => Item;

    public virtual void SetInfo(Player player, Item item)
    {
        Player = player;
        Item = item;
    }

    public virtual void Use()
    {

    }
}
