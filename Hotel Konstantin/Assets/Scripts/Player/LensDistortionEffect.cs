using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LensDistortionEffect : MonoBehaviour
{
    [SerializeField] private PostProcessProfile PostProcess;
    private LensDistortion LensDistortion = null;
    private float BaseValue = 0;

    private void OnApplicationQuit()
    {
        LensDistortion.intensity.value = BaseValue;
    }

    private void Start()
    {
        foreach(PostProcessEffectSettings settings in PostProcess.settings)
        {
            if(settings is LensDistortion)
            {
                LensDistortion = settings as LensDistortion;
                break;
            }
        }

        BaseValue = LensDistortion.intensity.value;
    }

    public void SetEffect(float target, float speed, float duraction)
    {
        StopAllCoroutines();
        StartCoroutine(Effect(target, speed, duraction));
    }

    private IEnumerator Effect(float target, float speed, float duraction)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        float direction = Mathf.Sign(target - LensDistortion.intensity.value);

        while (true)
        {
            duraction -= Time.deltaTime;

            LensDistortion.intensity.value += direction * Time.deltaTime * speed;

            if (Mathf.Sign(target - LensDistortion.intensity.value) != direction)
            {
                LensDistortion.intensity.value = target;
                break;
            }

            yield return waitForEndOfFrame;
        }

        yield return new WaitForSeconds(duraction);

        target = BaseValue;
        direction = Mathf.Sign(target - LensDistortion.intensity.value);

        while (true)
        {
            duraction -= Time.deltaTime;

            LensDistortion.intensity.value += direction * Time.deltaTime * speed;

            if (Mathf.Sign(target - LensDistortion.intensity.value) != direction)
            {
                LensDistortion.intensity.value = target;
                break;
            }

            yield return waitForEndOfFrame;
        }
    }
}
