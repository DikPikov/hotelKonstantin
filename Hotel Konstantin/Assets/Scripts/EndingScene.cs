using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class EndingScene : MonoBehaviour
{
    [SerializeField] private RectTransform Titles;
    [SerializeField] private GameObject ExitText;
    [SerializeField] private GameObject Fading;
    [SerializeField] private float Distance;
    [SerializeField] private float SpeedMultiplier;
    [SerializeField] private float Speed;

    [SerializeField] private int Ending;
    private bool Going = false;

    public void SetMultiplier(float value) { SpeedMultiplier = value; }
    public void Exit()
    {
        if(Distance == -228 && !Going)
        {
            Going = true;
            StartCoroutine(GoToMenu());
        }
    }


    private void Start()
    {
        Ending = PlayerPrefs.GetInt("End");

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Distance > 0)
        {
            float speed = Time.deltaTime * SpeedMultiplier * Speed;
            Distance -= speed;
            Titles.anchoredPosition += Vector2.up * speed;
        }
        else if (Distance != -228)
        {
            Distance = -228;
            ExitText.SetActive(true);
        }
    }

    private IEnumerator GoToMenu()
    {
        Fading.SetActive(true);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(0);
    }
}
