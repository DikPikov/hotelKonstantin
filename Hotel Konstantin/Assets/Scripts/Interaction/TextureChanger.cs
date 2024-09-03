using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    [SerializeField] private Texture NewTexture;
    void Start()
    {
        GetComponent<Renderer>().material.SetTexture("_MainTex", NewTexture);
    }
}
