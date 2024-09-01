using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Image StaminaBar;
    [SerializeField] private Image SanityBar;

    [SerializeField] private Player Player;

    private void Start()
    {
        Player.OnChanges += UpdateInfo;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        StaminaBar.fillAmount = Player._Stamina / 5f;
        SanityBar.fillAmount = Player._Sanity;
    }
}
