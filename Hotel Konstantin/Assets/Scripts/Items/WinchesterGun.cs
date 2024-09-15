using UnityEngine;
using System.Collections;

public class WinchesterGun : ItemObject
{
    [SerializeField] private GameObject EffectPrefab;
    [SerializeField] private GameObject GilzaPrefab;

    [SerializeField] private AudioSource PatronSound;
    [SerializeField] private AudioSource ShotSound;
    [SerializeField] private AudioSource PickupSound;

    [SerializeField] private Transform ShotPoint;
    [SerializeField] private Transform GilzaPoint;

    [SerializeField] private Winchester Winchester = null;

    private WinchesterReload WinchesterReload= null;

#if UNITY_ANDROID
    private bool ShootingNow = false;
#endif

    public override void SetInfo(Player player, Item item)
    {
        base.SetInfo(player, item);
        Winchester = item as Winchester;

        if (Winchester._Ready)
        {
            Animator.Play("Ready");
        }
        else
        {
            if (Winchester._Ammo > 0)
            {
                Animator.Play("Pickup");
            }
        }
    }

    private void Start()
    {
        if(Item == null)
        {
            SetInfo(Player, Winchester);
        }

        WinchesterReload = FindObjectOfType<WinchesterReload>();
    }

    public override void Use(bool state)
    {
        if (state)
        {
            if (Winchester._Ready)
            {
#if UNITY_ANDROID
                ShootingNow = true;
#endif
                StopAllCoroutines();
                StartCoroutine(Shooting());
            }
        }
        else
        {
#if UNITY_ANDROID
            ShootingNow = false;
#endif
        }
    }

    private void Update()
    {
        if (InputManager.GetButtonDown(InputManager.ButtonEnum.Reload))
        {
            StartReload();
        }
    }

    public void StartReload()
    {
        if (StaticTools.ContainsType(Player._Items, typeof(Bullet)) > -1 && Winchester._Ammo < 7)
        {
            Winchester._Ready = false;
            Animator.Play("Reload");
            Animator.SetBool("Reloading", true);
        }
        else if (!Winchester._Ready && Winchester._Ammo > 0 && !Animator.GetBool("Reloading"))
        {
            Animator.Play("Pickup");
        }
    }

    public void Reload()
    {
        Winchester._Ammo++;

        PatronSound.pitch = Random.Range(0.9f, 1.1f);
        PatronSound.Play();

        int index = StaticTools.ContainsType(Player._Items, typeof(Bullet));
        Player.ApplyItem(Player._Items[index], true);

        if (StaticTools.ContainsType(Player._Items, typeof(Bullet)) > -1 && Winchester._Ammo < 7)
        {
            Animator.SetBool("Reloading", true);
        }
        else
        {
            Animator.SetBool("Reloading", false);
        }
    }

    public void SetReady()
    {
        PickupSound.pitch = Random.Range(0.9f, 1.1f);
        PickupSound.Play();
        Winchester._Ready = true;

        if (Winchester._Shooted)
        {
            Transform gilza = Instantiate(GilzaPrefab, GilzaPoint.position, GilzaPoint.rotation).transform;
            gilza.GetComponent<Rigidbody>().AddForce(gilza.right * 5, ForceMode.Impulse);
            Winchester._Shooted = false;
        }
    }

    public void Shot()
    {
        if(Animator.GetInteger("ShotIncome") == 1)
        {
            return;
        }

        ShotSound.Play();

        Winchester._Shooted = true;

        Winchester._Ammo--;
        Winchester._Ready = false;

        WinchesterReload.CheckButton();

        Instantiate(EffectPrefab, ShotPoint.position, ShotPoint.rotation);

        if (Winchester._Ammo > 0)
        {
            Animator.SetInteger("ShotIncome", 0);
        }
        else
        {
            Animator.SetInteger("ShotIncome", 2);
        }
    }

    private IEnumerator Shooting()
    {
        Animator.Play("Shoot");
        Animator.SetInteger("ShotIncome", -1);

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

#if UNITY_ANDROID
        while (ShootingNow)
#else
        while (InputManager.GetButton(InputManager.ButtonEnum.Interact))
#endif
        {
            yield return waitForEndOfFrame;
        }

        if (!Winchester._Shooted)
        {
            Animator.SetInteger("ShotIncome", 1);
        }
    }
}
