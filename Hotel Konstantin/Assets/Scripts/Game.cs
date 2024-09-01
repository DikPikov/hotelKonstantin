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

    public static float _HotelTime
    {
        get
        {
            return Instance.HotelTime;
        }
        set
        {
            Instance.HotelTime = value;
            Instance.TimeIndicator.text = $"{(int)(value / 3600f)}ч {(int)(value % 3600 / 60f)}м {(int)(value % 60)}с";
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
            HotelTime++;
            TimeIndicator.text = $"{(int)(HotelTime / 3600f)}ч {(int)(HotelTime % 3600 / 60f)}м {(int)(HotelTime % 60)}с";
            yield return new WaitForSeconds(1);
        }

    }
}
