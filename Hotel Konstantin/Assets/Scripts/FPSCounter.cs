using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private Text Text;

    private void Start()
    {
        StartCoroutine(Count());
    }
     
    private IEnumerator Count()
    {
        while (true)
        {
            Text.text = $"{(int)(1 / Time.unscaledDeltaTime)}";
            yield return new WaitForSecondsRealtime(0.15f);
        }
    }
}
