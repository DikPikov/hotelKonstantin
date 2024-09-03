using System.Collections;
using UnityEngine;

public class FOVEffect : MonoBehaviour
{
    [SerializeField] private Camera Camera;

    public void SetEffect(float target, float speed, float duraction)
    {
        StopAllCoroutines();
        StartCoroutine(Effect(target, speed, duraction));
    }

    private IEnumerator Effect(float target, float speed, float duraction)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        float direction = Mathf.Sign(target - Camera.fieldOfView);

        while (true)
        {
            duraction -= Time.deltaTime;

            Camera.fieldOfView += direction * Time.deltaTime * speed;

            if (Mathf.Sign(target - Camera.fieldOfView) != direction)
            {
                Camera.fieldOfView = target;
                break;
            }

            yield return waitForEndOfFrame;
        }

        yield return new WaitForSeconds(duraction);

        target = 75;
        direction = Mathf.Sign(target - Camera.fieldOfView);

        while (true)
        {
            Camera.fieldOfView += direction * Time.deltaTime * speed;

            if (Mathf.Sign(target - Camera.fieldOfView) != direction)
            {
                Camera.fieldOfView = target;
                break;
            }

            yield return waitForEndOfFrame;
        }
    }
}
