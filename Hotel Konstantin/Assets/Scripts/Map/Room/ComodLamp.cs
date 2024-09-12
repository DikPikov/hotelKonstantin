using System.Collections.Generic;
using UnityEngine;

public class ComodLamp : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] Model;
    [SerializeField] private GameObject Light;
    [SerializeField] private Room Room;

    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private int MaterialIndex;
    [SerializeField] private Material LightMaterial;
    [SerializeField] private Material LightOffMaterial;
    [SerializeField] private bool TurndOn;

    public float _BeforeTime => 0.3f;
    public bool _CanInteract => true;

    public bool _On
    {
        get
        {
            return TurndOn;
        }
        set
        {
            TurndOn = value;
            Light.SetActive(TurndOn);

            Room.UpdateTaskInfo();

            if (TurndOn)
            {
                List<Material> materials = new List<Material>();
                Renderer.GetMaterials(materials);

                materials[MaterialIndex] = LightMaterial;
                Renderer.SetMaterials(materials);
            }
            else
            {
                List<Material> materials = new List<Material>();
                Renderer.GetMaterials(materials);

                materials[MaterialIndex] = LightOffMaterial;
                Renderer.SetMaterials(materials);
            }
        }
    }

    private void Start()
    {
        int random = Random.Range(0, Model.Length);
        Model[random].SetActive(true);

        for(int i = 0; i < Model.Length; i++)
        {
            if(i != random)
            {
                Destroy(Model[i]);
            }
        }

        Renderer = Model[random].GetComponent<MeshRenderer>();

        _On = TurndOn;
    }

    public void Interact() => _On = !TurndOn;
}
