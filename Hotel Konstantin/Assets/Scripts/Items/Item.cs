using UnityEngine;

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

public class Alcohol : Item
{
    public override string _Prefab => "Alcohol";
}
