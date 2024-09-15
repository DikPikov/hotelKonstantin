using UnityEngine;
using System.Collections;

public class Ambienter : MonoBehaviour
{
    [SerializeField] private AudioClip[] Ambients;
    [SerializeField] private AudioSource AudioSource;

    private void Start()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        while (true)
        {
            yield return new WaitForSeconds(AudioSource.clip.length + Random.Range(10, 20f));

            AudioSource.clip = Ambients[Random.Range(0, Ambients.Length)];
            AudioSource.pitch = Random.Range(0.9f, 1.1f);
            AudioSource.Play();
        }
    }
}
