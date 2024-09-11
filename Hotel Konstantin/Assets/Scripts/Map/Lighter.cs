using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : MonoBehaviour
{
    [SerializeField] private Light Light;

    [SerializeField] private Renderer LampRenderer;
    [SerializeField] private int LightMaterialIndex;
    private Material LampMaterial = null;

    [SerializeField] private bool Enabled = true;

    [SerializeField] private float Value;

    [SerializeField] private float Intensity;
    [SerializeField] private float Range;

    public bool _Enabled
    {
        get
        {
            return Enabled;
        }
        set
        {
            Enabled = value;

            if (Enabled)
            {
                if (_LampMaterial != null)
                {
                    _LampMaterial.color = Light.color * Mathf.Min(Intensity, Value);
                }
            }
            else
            {
                if (_LampMaterial != null)
                {
                    _LampMaterial.color = Color.black;
                }
            }
        }
    }

    public int _LightMaterialIndex
    {
        get
        {
            return LightMaterialIndex;
        }
        set
        {
            LightMaterialIndex = value;
        }
    }

    public Renderer _Renderer
    {
        get
        {
            return LampRenderer;
        }
        set
        {
            LampRenderer = value;

            LampMaterial = null;
            Material material = _LampMaterial;

            _Value = Value;
        }
    }

    private Material _LampMaterial
    {
        get
        {
            if(LampRenderer == null)
            {
                return LampMaterial;
            }

            if (LampMaterial == null)
            {
                LampMaterial = new Material(LampRenderer.materials[LightMaterialIndex]);
                LampRenderer.materials[LightMaterialIndex] = LampMaterial;

                LampMaterial.name = "Light";

                List<Material> materials = new List<Material>();
                LampRenderer.GetMaterials(materials);

                materials[LightMaterialIndex] = LampMaterial;
                LampRenderer.SetMaterials(materials);
            }

            return LampMaterial;
        }
    }

    public Color _Color
    {
        get
        {
            return Light.color;
        }
        set
        {
            Light.color = value;

            if (gameObject.activeSelf)
            {
                if (_LampMaterial != null)
                {
                    _LampMaterial.color = value * Mathf.Min(Intensity, Value) * Enabled.GetHashCode();
                }
            }
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
            Light.intensity = Intensity * Value * Enabled.GetHashCode();

            if (gameObject.activeSelf)
            {
                if (_LampMaterial != null)
                {
                    _LampMaterial.color = Light.color * Mathf.Min(Intensity, Value) * Enabled.GetHashCode();
                }
            }
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

            Light.intensity = Intensity * Value * Enabled.GetHashCode();
            Light.range = Range * Value;

            if (gameObject.activeSelf)
            {
                if (_LampMaterial != null)
                {
                    _LampMaterial.color = Light.color * Mathf.Min(Intensity, Value) * Enabled.GetHashCode();
                }
            }
        }
    }
}
