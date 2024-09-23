using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CosmareImages : MonoBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private Sprite[] Cripys;

    public void Show(float time)
    {
        StopAllCoroutines();
        StartCoroutine(ShowImage(time));
    }

    private IEnumerator ShowImage(float time)
    {
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        AudioSource.Play();
        Image.sprite = Cripys[Random.Range(0, Cripys.Length)];
        Image.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(time);

        Image.gameObject.SetActive(false);
    }
}