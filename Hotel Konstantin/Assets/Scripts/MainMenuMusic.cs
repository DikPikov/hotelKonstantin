using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusic : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSource;

    private static GameObject Instance;

    private int MySceneIndex = -1;

    public void CheckScene(Scene last, Scene current)
    {
        if(current.buildIndex != MySceneIndex)
        {
            Instance = null;
            StartCoroutine(Fade());
        }
    }

    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.activeSceneChanged += CheckScene;

        MySceneIndex = SceneManager.GetActiveScene().buildIndex;

        Instance = gameObject;
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Fade()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while(AudioSource.volume > 0)
        {
            AudioSource.volume -= Time.unscaledDeltaTime * 0.6f;
            yield return waitForEndOfFrame;
        }

        Destroy(gameObject);

        SceneManager.activeSceneChanged -= CheckScene;
    }
}
