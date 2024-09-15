using UnityEngine;

public class Pause : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private bool LockMouse;
#endif
    [SerializeField] private bool StartPaused;

    private static Pause Instance;
    private bool Paused = false;

    public static bool _Paused
    {
        get
        {
            return Instance.Paused;
        }
        set
        {
            Instance.i_SetPause(value);
        }
    }

    private void Awake()
    {
        Instance = this;
        i_SetPause(StartPaused);
    }

    public void i_SetPause(bool value)
    {
        Paused = value;

        Time.timeScale = (!Paused).GetHashCode();

#if UNITY_EDITOR
        if (Paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            if (LockMouse)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
#elif UNITY_ANDROID
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#else
        if (Paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
#endif
    }
}