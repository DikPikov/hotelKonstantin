using UnityEngine;

public class WinchesterReload : MonoBehaviour
{
    [SerializeField] private GameObject Button;
    [SerializeField] private Player Player;

    private void Start()
    {
        Player.OnItemChanges += CheckButton;
        CheckButton();
    }

    public void CheckButton()
    {
        Button.SetActive(false);

        WinchesterGun gun = Player._CurrentItem as WinchesterGun;
        if (gun != null)
        {
            if(StaticTools.ContainsType(Player._Items, typeof(Bullet)) > -1 && (gun._Item as Winchester)._Ammo < 7)
            {
                Button.SetActive(true);
            }
        }
    }

    public void Reload()
    {
        if(Player._CurrentItem != null && Player._CurrentItem is WinchesterGun)
        {
            (Player._CurrentItem as WinchesterGun).StartReload();
        }
    }
}
