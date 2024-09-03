using UnityEngine;
using System.Collections;

public interface ILiftable 
{ 
    public Floor _Floor { get; set; }
    public Transform _Transform { get; }
}

public class Lift : MonoBehaviour
{
    [SerializeField] private Floor[] Floors;

    [SerializeField] private Animator Animator;

    [SerializeField] private Material IndicatorMaterial;
    [SerializeField] private Texture2D[] IndicatorTextures;

    [SerializeField] private int CurrentFloor;
    [SerializeField] private float Speed;

    [SerializeField] private int[] Order = new int[0];

    private ILiftable[] Objects = new ILiftable[0];

    private Coroutine ElevateCoroutine = null;

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
            ElevateCoroutine = StartCoroutine(Elevate(Order[0] * 5 + 1.1f));
        }
    }

    public void ObjectEntered(Transform object1)
    {
        ILiftable liftable = object1.GetComponentInParent<ILiftable>();
        if (liftable != null)
        {
            liftable._Transform.parent = transform;
        }
    }
    public void ObjectLeft(Transform object1)
    {
        ILiftable liftable = object1.GetComponentInParent<ILiftable>();
        if (liftable != null)
        {
            liftable._Transform.parent = null;
        }
    }

    private IEnumerator Elevate(float high)
    {
        float sign = Mathf.Sign(high - transform.position.y);
        int floor = CurrentFloor;

        Animator.SetBool("Open", false);
        yield return new WaitForSeconds(2);

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        while (true)
        {
            transform.position += Vector3.up * (sign * Time.deltaTime * Speed);

            floor = (int)((transform.position.y - 0.5f) / 5);
            if(floor != CurrentFloor)
            {
                CurrentFloor = floor;

                IndicatorMaterial.SetTexture("_MainTex", IndicatorTextures[floor]);
                IndicatorMaterial.SetTexture("_EmissionMap", IndicatorTextures[floor]);

                foreach (ILiftable liftable in Objects)
                {
                    liftable._Floor = Floors[floor];
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

        yield return new WaitForSeconds(2);

        Order = StaticTools.ReduceMassive(Order, 0);

        if(Order.Length > 0)
        {
            ElevateCoroutine = StartCoroutine(Elevate(Order[0] * 5 + 1.1f));
        }
        else
        {
            ElevateCoroutine = null;
        }
    }
}
