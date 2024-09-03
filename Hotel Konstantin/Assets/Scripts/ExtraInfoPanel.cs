using UnityEngine;
using UnityEngine.UI;

public class ExtraInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject Panel;

    private void Update()
    {
        Panel.SetActive(InputManager.GetButton(InputManager.ButtonEnum.OpenExtraInfo));
    }
}
