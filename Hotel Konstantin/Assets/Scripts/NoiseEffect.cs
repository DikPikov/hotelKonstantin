using UnityEngine;

public class NoiseEffect : MonoBehaviour
{
    [SerializeField] private Material Noise;

    private void Update()
    {
        Noise.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
