using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Game._GameOver)
        {
            return;
        }

        PlayerInterface.SetActive(!state);
        MenuPanel.SetActive(state);
        Pause._Paused = state;
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene(0);
#else
                SceneManager.LoadScene(0);
#endif
    }
}