using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Televizor : MonoBehaviour, IInteractable
{
    [SerializeField] private Room Room;
    [SerializeField] private AudioSource Noise;
    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private int DisplayMaterialIndex;
    [SerializeField] private Material[] Materials;
    [SerializeField] private bool TurnedOn;

    public float _BeforeTime => 0.5f;
    public bool _CanInteract => true;

    public bool _On
    {
        get
        {
            return TurnedOn;
        }
        set
        {
            TurnedOn = value;

            Noise.volume = value.GetHashCode();

            List<Material> materials = new List<Material>();
            Renderer.GetMaterials(materials);

            materials[DisplayMaterialIndex] = Materials[TurnedOn ? 0 : 1];
            Renderer.SetMaterials(materials);

            Room.UpdateTaskInfo();
        }
    }

    private void Start()
    {
        _On = TurnedOn;
    }

    public void Interact() => _On = !TurnedOn;
}
