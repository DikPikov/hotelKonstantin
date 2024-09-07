using UnityEngine;
using UnityEngine.UI;

public class ExtraInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    private bool hideFlag = true;

    private void Update()
    {
        if (InputManager.GetButtonDown(InputManager.ButtonEnum.OpenExtraInfo)){ if (hideFlag) { hideFlag = false; return;} else {hideFlag = true;} }
        Panel.SetActive(hideFlag);
    }
}
