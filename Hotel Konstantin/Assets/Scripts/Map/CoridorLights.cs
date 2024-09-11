using UnityEngine;
using System.Collections;

public class CoridorLights : MonoBehaviour
{
    [SerializeField] private Floor Floor;
    [SerializeField] private Lighter[] Lighters;

    public void Enable(bool state)
    {
        StopAllCoroutines();

        foreach (Lighter lighter in Lighters)
        {
            lighter._Value = state.GetHashCode();
        }

        if (state)
        {
            Floor.SpawnGhost(true);
        }
        else
        {
            Floor.SpawnGhost(false);
        }
    }

    public void DistortLight()
    {
        StopAllCoroutines();
        StartCoroutine(LightDistorting());
    }

    public void AddLighter(Lighter lighter)
    {
        Lighters = StaticTools.ExpandMassive(Lighters, lighter);
    }

    private IEnumerator LightDistorting()
    {
        bool state = true;
        while(true)
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
