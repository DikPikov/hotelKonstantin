
using UnityEngine;
using UnityEngine.UI;

public class ExtraInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Settings Settings;
    private bool AlwaysShow = true;

#if !UNITY_ANDROID
    private void Start()
    {
        Settings.OnChanges += CheckAlwaysShow;
        CheckAlwaysShow();
    }

    private void CheckAlwaysShow()
    {
        AlwaysShow = Settings._Config.AlwaysShowInterface;
    }

    private void Update()
    {
        if (AlwaysShow)
        {
            Panel.SetActive(true);
        }
        else
        {
            Panel.SetActive(InputManager.GetButton(InputManager.ButtonEnum.OpenExtraInfo));
        }
    }
#endif
}