using UnityEngine;

public class Player : MonoBehaviour, ILiftable
{
    [SerializeField] private Floor Floor;

    [SerializeField] private float Sanity;
    [SerializeField] private float Stamina;

    public event SimpleVoid OnChanges = null;

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
}
