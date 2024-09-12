using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SanityTemperature : MonoBehaviour
{
    [SerializeField] private PostProcessProfile Profile;
    [SerializeField] private Player Player;
    private ColorGrading ColorGrading = null;
    private Grain Grain = null;

    public void UpdateTemperature(float value)
    {
        if(ColorGrading == null || Grain == null)
        {
            foreach (var setting in Profile.settings)
            {
                if (setting is ColorGrading)
                {
                    ColorGrading = setting as ColorGrading;
                }
                if (setting is Grain)
                {
                    Grain = setting as Grain;
                }
            }
        }

        Grain.intensity.value = Mathf.Lerp(0.5f, 0, 1- value);
        Grain.size.value = Mathf.Lerp(1.5f, 0.3f, 1 - value);

        ColorGrading.temperature.value = Mathf.Lerp(30, -5, 1 - value);
    }
}
