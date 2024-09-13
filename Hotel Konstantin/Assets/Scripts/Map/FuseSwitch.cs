using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class FuseSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Player Player;
    [SerializeField] private GameObject[] FuseObjects;
    [SerializeField] private RoomsFloor Floor;
    private Fuse Fuse = null;

    public Fuse _Fuse
    {
        get
        {
            return Fuse;
        }
        set
        {
            SetFuseNoNotify(value);
            GameMap._MapLights.FloorFuseUpdate(Floor);
        }
    }
    public float _BeforeTime => 1;
    public int _State
    {
        get
        {
            if(Fuse is StableFuse)
            {
                return 2;
            }
            else if(Fuse is DamagedFuse)
            {
                return 1;
            }

            return 0;
        }
    }
    public bool _CanInteract
    {
        get
        {
            if(Fuse != null)
            {
                return true;
            }

            if (Player._CurrentItem != null && Player._CurrentItem._Item is Fuse)
            {
                return true;
            }

            return false;
        }
    }

   public void Interact()
    {
        if(Fuse != null)
        {
            if(Player.ApplyItem(Fuse, false))
            {
                _Fuse = null;
            }
            return;
        }

        if(Player._CurrentItem != null)
        {
            Fuse fuse = Player._CurrentItem._Item as Fuse;
            if (fuse != null)
            {
                _Fuse = fuse;
                Player.ApplyItem(fuse, true);
            }
        }
    }

    public void SetFuseNoNotify(Fuse fuse)
    {
        Fuse = fuse;

        switch (_State)
        {
            case 0:
                FuseObjects[0].SetActive(false);
                FuseObjects[1].SetActive(false);
                break;
            case 1:
                FuseObjects[0].SetActive(false);
                FuseObjects[1].SetActive(true);
                break;
            case 2:
                FuseObjects[0].SetActive(true);
                FuseObjects[1].SetActive(false);
                break;
        }
    }
}
