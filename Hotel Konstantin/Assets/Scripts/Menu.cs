using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject Panel;

    private void Update()
    {
        if (InputManager.GetButtonDown(InputManager.ButtonEnum.Menu))
        {
            ShowPanel(!Panel.activeSelf);
        }
    }

    public void ShowPanel(bool state)
    {
        Panel.SetActive(state);
        Pause._Paused = state;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}