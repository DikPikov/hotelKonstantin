using UnityEngine;
using System.Collections;

public interface ILiftable 
{ 
    public Floor _Floor { get; set; }
    public Transform _Transform { get; }
}

public class Lift : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    [SerializeField] private AudioSource LiftingNoise;
    [SerializeField] private AudioSource[] Source;
    [SerializeField] private GameObject[] ControlPanels;

    [SerializeField] private Material IndicatorMaterial;
    [SerializeField] private Texture2D[] IndicatorTextures;

    [SerializeField] private int CurrentFloor;
    [SerializeField] private float Speed;

    [SerializeField] private int[] Order = new int[0];

    private ILiftable[] Objects = new ILiftable[0];

    private Coroutine ElevateCoroutine = null;

    private void Start()
    {
        LiftingNoise.volume = 0;

        IndicatorMaterial.SetTexture("_MainTex", IndicatorTextures[CurrentFloor]);
        IndicatorMaterial.SetTexture("_EmissionMap", IndicatorTextures[CurrentFloor]);
    }

    public void PlaySource(int index)
    {
        Source[index].Play();
    }

    public void SetControlPanel(int index)
    {
        for(int i = 0; i < ControlPanels.Length; i++)
        {
            ControlPanels[i].SetActive(i == index);
        }
    }

    public void Elevate(int floor)
    {
        if(ElevateCoroutine == null)
        {
            if(CurrentFloor == floor)
            {
                return;
            }
        }

        if(Order.Length > 1)
        {
            Order[1] = floor;
        }
        else
        {
            Order = StaticTools.ExpandMassive(Order, floor);
        }

        if(ElevateCoroutine == null)
        {
            ElevateCoroutine = StartCoroutine(ElevateProcess(Order[0]));
        }
    }

    public void ObjectEntered(Transform object1)
    {
        ILiftable liftable = object1.GetComponentInParent<ILiftable>();
        if (liftable != null)
        {
            liftable._Transform.parent = transform;
            Objects = StaticTools.ExpandMassive(Objects, liftable);
        }
    }
    public void ObjectLeft(Transform object1)
    {
        ILiftable liftable = object1.GetComponentInParent<ILiftable>();
        if (liftable != null)
        {
            liftable._Transform.parent = null;
            Objects = StaticTools.RemoveFromMassive(Objects, liftable);
        }
    }

    private int CheckFloor(float high)
    {
        for(int i = 0; i < GameMap._Floors.Length - 1; i++)
        {
            if(high >= GameMap._Floors[i].transform.position.y && high < GameMap._Floors[i + 1].transform.position.y)
            {
                return i;
            }
        }

        return GameMap._Floors.Length - 1;
    }

    private IEnumerator ElevateProcess(int targetFloor)
    {
        float high = GameMap._Floors[targetFloor].transform.position.y + 1f;
        float sign = Mathf.Sign(high - transform.position.y);
        int floor = CurrentFloor;

        Animator.SetBool("Open", false);
        yield return new WaitForSeconds(2);

        LiftingNoise.volume = 1;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        while (true)
        {
            transform.position += Vector3.up * (sign * Time.deltaTime * Speed);

            floor = CheckFloor(transform.position.y);
            if(floor != CurrentFloor)
            {
                CurrentFloor = floor;

                IndicatorMaterial.SetTexture("_MainTex", IndicatorTextures[floor]);
                IndicatorMaterial.SetTexture("_EmissionMap", IndicatorTextures[floor]);

                foreach (ILiftable liftable in Objects)
                {
                    liftable._Floor = GameMap._Floors[floor];
                }
            }

            if(Mathf.Sign(high - transform.position.y) != sign)
            {
                transform.position = new Vector3(transform.position.x, high, transform.position.z);
                break;
            }

            yield return waitForEndOfFrame;
        }

        Animator.SetBool("Open", true);

        LiftingNoise.volume = 0;

        yield return new WaitForSeconds(2);

        Order = StaticTools.ReduceMassive(Order, 0);

        if(Order.Length > 0)
        {
            ElevateCoroutine = StartCoroutine(ElevateProcess(Order[0]));
        }
        else
        {
            ElevateCoroutine = null;
        }
    }
}
