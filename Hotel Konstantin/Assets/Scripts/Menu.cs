using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject PlayerInterface;
    [SerializeField] private GameObject MenuPanel;

    private void Update()
    {
        if (InputManager.GetButtonDown(InputManager.ButtonEnum.Menu))
        {
            ShowPanel(!MenuPanel.activeSelf);
        }
    }

    public void ShowPanel(bool state)
    {
        PlayerInterface.SetActive(!state);
        MenuPanel.SetActive(state);
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