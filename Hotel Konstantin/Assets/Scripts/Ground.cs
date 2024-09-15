using UnityEngine;

public class Ground : MonoBehaviour
{
    public enum GroundMaterial {Wood, Metal, Beton}

    [SerializeField] private GroundMaterial Material;

    public GroundMaterial _Material => Material;
}
