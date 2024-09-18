using UnityEngine;


public class LiftSound : MonoBehaviour
{
    [SerializeField] private AudioSource LiftingNoise;
    [SerializeField] private AudioSource[] Source;

    public float _Volume
    {
        get
        {
            return LiftingNoise.volume;
        }
        set
        {
            LiftingNoise.volume = value;
        }
    }

    private void Start()
    {
        LiftingNoise.volume = 0;
    }

    public void PlaySource(int index)
    {
        Source[index].Play();
    }
}
