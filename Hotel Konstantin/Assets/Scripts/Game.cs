using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Text TimeIndicator;
    [SerializeField] private float HotelTime;

    private bool GameOver = false;

    private static Game Instance;

    public static float _HotelMadness => Mathf.Clamp01(Instance.HotelTime / 1800);
    public static float _HotelTime
    {
        get
        {
            return Instance.HotelTime;
        }
        set
        {
            Instance.HotelTime = value;

            int hours = (int)(value / 3600f);
            int minuts = (int)(value % 3600 / 60f);
            int seconds = (int)(value % 60);

            Instance.TimeIndicator.text = $"{(hours > 0 ? $"{(hours < 10 ? $"0{hours}" : $"{hours}")} : " : $"")}{(minuts < 10 ? $"0{minuts}" : $"{minuts}")} : {(seconds < 10 ? $"0{seconds}" : $"{seconds}")}";
        }
    }
    public static bool _GameOver
    {
        get
        {
            return Instance.GameOver;
        }
    }

    private void Awake()
    {
        Instance = this;

        StartCoroutine(Timer());
    }

    public static void Over()
    {
        Instance.GameOver = true;
        Instance.Panel.SetActive(true);
        Pause._Paused = true;
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            _HotelTime++;
            yield return new WaitForSeconds(1);
        }

    }
}
