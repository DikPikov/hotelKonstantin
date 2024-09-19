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
    [SerializeField] private Player Player;
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
        set
        {
            Instance.GameOver = value;

            Instance.Player.AnimateDeath(value);
        }
    }

    private void Awake()
    {
        Instance = this;

        StartCoroutine(Timer());
    }

    public void Revive()
    {
        _GameOver = false;

        Instance.Panel.SetActive(false);
        Pause._Paused = false;

        bool hasFloor = false;
        for (int i = 0; i < GameMap._RoomFloors.Length; i++)
        {
            if (GameMap._BasementFloor._Fuses[i]._State != 0)
            {
                hasFloor = true;
                break;
            }
        }

        if (hasFloor)
        {
            int floor = Random.Range(0, GameMap._RoomFloors.Length);

            while (GameMap._BasementFloor._Fuses[floor]._State == 0 || GameMap._RoomFloors[floor]._Rooms.Length <= 0)
            {
                floor = Random.Range(0, GameMap._RoomFloors.Length);
            }

            int room = Random.Range(0, GameMap._RoomFloors[floor]._Rooms.Length);

            GameMap._RoomFloors[floor]._Rooms[room]._RoomLight._Enabled = true;
            Player.transform.position = GameMap._RoomFloors[floor]._Rooms[room].transform.position + Vector3.up;
            Player._Floor = GameMap._RoomFloors[floor];
            FindObjectOfType<MapLights>().SetFloorState(GameMap._RoomFloors[floor], 0);
        }
        else
        {
            Player.transform.position = GameMap._BasementFloor.transform.position + Vector3.up;
            Player._Floor = GameMap._BasementFloor;
        }

        _HotelTime += Random.Range(300, 900);

        Monster monster = FindObjectOfType<Monster>();
        monster.SetupBehavior(monster.RandomWalk());
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
