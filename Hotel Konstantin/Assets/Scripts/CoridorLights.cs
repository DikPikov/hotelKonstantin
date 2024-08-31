using UnityEngine;
using System.Collections;

public class CoridorLights : MonoBehaviour
{
    [SerializeField] private Floor Floor;
    [SerializeField] private Lighter[] Lighters;
    [SerializeField] private float LightTime;

    public float _LightTime
    {
        get
        {
            return LightTime;
        }
        set
        {
            LightTime = value;

            Floor.SpawnGhost(true);

            StopAllCoroutines();
            StartCoroutine(LightOff());
        }
    }

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(LightOff());
    }

    private IEnumerator LightOff()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        foreach (Lighter lighter in Lighters)
        {
            lighter._Value = 1;
        }

        Coroutine distorting = null;

        while(LightTime > 0)
        {
            LightTime -= Time.deltaTime;

            if(LightTime < 30 && distorting == null)
            {
                distorting = StartCoroutine(LightDistorting());
            }

            yield return waitForEndOfFrame;
        }

        foreach (Lighter lighter in Lighters)
        {
            lighter._Value = 0;
        }

        Floor.SpawnGhost(false);
    }

    private IEnumerator LightDistorting()
    {
        bool state = true;
        while(LightTime > 0 && LightTime < 30)
        {
            state = !state;

            foreach (Lighter lighter in Lighters)
            {
                lighter._Value = state ? 1 : 0.5f ;
            }
            yield return new WaitForSeconds(Random.Range(0.05f, 0.6f));
        }
    }
}
