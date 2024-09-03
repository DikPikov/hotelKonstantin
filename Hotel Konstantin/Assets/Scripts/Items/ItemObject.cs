using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] protected Animator Animator;
    protected Player Player;
    protected Item Item;

    public Item _Item => Item;

    public void SetInfo(Player player, Item item)
    {
        Player = player;
        Item = item;
    }

    public virtual void Use()
    {

    }
}
