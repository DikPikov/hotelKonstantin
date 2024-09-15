using UnityEngine;

public class PanelPlatform : MonoBehaviour
{
    [SerializeField] private GameObject[] PanelComputer;
    [SerializeField] private GameObject[] PanelMobile;

    private void Awake()
    {
#if UNITY_ANDROID
        foreach(GameObject computer in PanelComputer)
        {
            computer.SetActive(false);
        }
        foreach (GameObject mobile in PanelMobile)
        {
            mobile.SetActive(true);
        }
#else
        foreach(GameObject computer in PanelComputer)
        {
            computer.SetActive(true);
        }
        foreach (GameObject mobile in PanelMobile)
        {
            mobile.SetActive(false);
        }
#endif
    }
}
