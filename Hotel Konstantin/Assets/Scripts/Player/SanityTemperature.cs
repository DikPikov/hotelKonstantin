using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SanityTemperature : MonoBehaviour
{
    [SerializeField] private PostProcessProfile Profile;
    [SerializeField] private Player Player;
    private ColorGrading ColorGrading = null;
    private Grain Grain = null;

    private void Start()
    {
        foreach (var setting in Profile.settings)
        {
            if (setting is ColorGrading)
            {
                ColorGrading = setting as ColorGrading;
            }
            if(setting is Grain)
            {
                Grain = setting as Grain;
            }
        }

        Player.OnChanges += UpdateTemperature;
        UpdateTemperature();
    }

    public void UpdateTemperature()
    {
        Grain.intensity.value = Mathf.Lerp(0.5f, 0, Player._Sanity);
        Grain.size.value = Mathf.Lerp(1.5f, 0.3f, Player._Sanity);

        ColorGrading.temperature.value = Mathf.Lerp(50, 0, Player._Sanity);
    }
}
