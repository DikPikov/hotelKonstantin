using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource Sound;

    public void Play()
    {
        Sound.pitch = Random.Range(0.9f, 1.1f);
        Sound.Play();
    }
}
