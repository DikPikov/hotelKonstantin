using UnityEngine;

public interface IUsable { }

public abstract class Item
{
    protected Sprite Icon;

    public virtual Sprite _Icon
    {
        get
        {
            if (Icon == null)
            {
                Icon = Resources.Load<Sprite>($"Icons/{_Prefab}");
            }

            return Icon;
        }
    }
    public virtual string _Prefab => "";
    public virtual float _PickTime => 0.3f;

   public static Item Create(string name)
    {
        return System.Activator.CreateInstance(System.Type.GetType(name)) as Item;
    }
}

public class Alcohol : Item, IUsable
{
    public override string _Prefab => "Alcohol";
}

public class Bullet : Item
{
    public override string _Prefab => "Patron";
}

public abstract class Fuse : Item { }

public class DamagedFuse : Fuse
{
    public override string _Prefab => "DamagedFuse";
}

public class StableFuse : Fuse
{
    public override string _Prefab => "StableFuse";
}

[System.Serializable]
public class Winchester : Item, IUsable
{
    [SerializeField] private int Ammo = 0;
    [SerializeField] private bool Ready = false;
    [SerializeField] private bool Shooted = false;

    public override string _Prefab => "Winchester";
    public override float _PickTime => 1.5f;

    public int _Ammo
    {
        get
        {
            return Ammo;
        }
        set
        {
            Ammo = Mathf.Clamp(value, 0, 7);
        }
    }
    public bool _Ready
    {
        get
        {
            return Ready;
        }
        set
        {
            Ready = value;
        }
    }
    public bool _Shooted
    {
        get
        {
            return Shooted;
        }
        set
        {
            Shooted = value;
        }
    }
}