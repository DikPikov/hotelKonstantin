using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator[] AnimatorObjects;
    private IEnumerator EntryNumerator()
    {
        AnimatorObjects[1].SetBool("isOpen", true);
        AnimatorObjects[0].SetBool("entrance", true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

    public void StartEntryToGame()
    {
        StartCoroutine(EntryNumerator());
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
