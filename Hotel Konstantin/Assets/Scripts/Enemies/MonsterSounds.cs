using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSounds : MonoBehaviour
{
    [SerializeField] private AudioSource WalkSound;
    [SerializeField] private AudioSource AttackSound;
    [SerializeField] private AudioSource SeekSound;
    [SerializeField] private AudioSource HelloSound;

    public void PlayWalkSound()
    {
        WalkSound.pitch = Random.Range(0.9f, 1.1f);
        WalkSound.Play();
    }

    public void PlayAttackSound()
    {
        AttackSound.pitch = Random.Range(0.9f, 1.1f);
        AttackSound.Play();
    }

    public void PlaySeekSound()
    {
        SeekSound.pitch = Random.Range(0.9f, 1.1f);
        SeekSound.Play();
    }

    public void SetHelloSound(float volume)
    {
        HelloSound.volume = volume;
    }
}
