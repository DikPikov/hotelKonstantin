using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements.Experimental;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject Fading;
    [SerializeField] private SanityTemperature SanityTemperature;
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

            Instance.SanityTemperature.UpdateTemperature(_HotelMadness);

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

    public void RunEnding(int index)
    {
        PlayerPrefs.SetInt("End", index);
        PlayerPrefs.Save();

        StartCoroutine(OpenEnding());
    }

    public static void Over()
    {
        Instance.GameOver = true;
        Instance.Panel.SetActive(true);
        Pause._Paused = true;
    }

    private IEnumerator OpenEnding()
    {
        Fading.SetActive(true);

        yield return new WaitForSecondsRealtime(1);

        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
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
