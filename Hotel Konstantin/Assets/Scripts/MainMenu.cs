using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator[] AnimatorObjects;

    [SerializeField] private Text Subtitle;
    [SerializeField] private int maxQuoteIndex;

    private void Start()
    {
        Subtitle.text = LocalizationSettings.StringDatabase.GetLocalizedString("LocalizationTable", "Quote"+Random.Range(0, maxQuoteIndex-1).ToString());
        
    }

    private IEnumerator EntryNumerator()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(1);
        loading.allowSceneActivation = false;

        AnimatorObjects[1].SetBool("isOpen", true);
        AnimatorObjects[0].SetBool("entrance", true);

            yield return new WaitForSeconds(1.8f);

        loading.allowSceneActivation = true;
    }

    public void StartEntryToGame()
    {
        StartCoroutine(EntryNumerator());
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
