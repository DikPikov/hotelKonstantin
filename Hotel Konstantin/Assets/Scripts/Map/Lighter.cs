using UnityEngine;

public class Lighter : MonoBehaviour
{
    [SerializeField] private Light Light;
    [SerializeField] private float Value;

    [SerializeField] private float Intensity;
    [SerializeField] private float Range;

    public Color _Color
    {
        get
        {
            return Light.color;
        }
        set
        {
            Light.color = value;
        }
    }
    public float _Intensity
    {
        get
        {
            return Intensity;
        }
        set
        {
            Intensity = value;
            Light.intensity = Intensity * Value;
        }
    }
    public float _Range
    {
        get
        {
            return Range;
        }
        set
        {
            Range = value;
            Light.range = Range * Value;
        }
    }

    public float _Value
    {
        get
        {
            return Value;
        }
        set
        {
            Value = value;

            Light.intensity = Intensity * Value;
            Light.range = Range * Value;
        }
    }
}
