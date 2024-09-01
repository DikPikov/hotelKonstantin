using UnityEngine;
using System.Collections;

public interface ILiftable 
{ 
    public Floor _Floor { get; set; }
}

public class Lift : MonoBehaviour
{
    [SerializeField] private Floor[] Floors;
    [SerializeField] private int CurrentFloor;
    [SerializeField] private float Speed;

    [SerializeField] private int[] Order = new int[0];

    private ILiftable[] Objects = new ILiftable[0];

    private Coroutine ElevateCoroutine = null;

    public void Elevate(int floor)
    {
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
            ElevateCoroutine = StartCoroutine(Elevate(Order[0] * 5 + 0.5f));
        }
    }

    public void ObjectEntered(Transform object1)
    {
        if(object1.GetComponent<ILiftable>() != null)
        {
            object1.parent = transform;
        }
    }
    public void ObjectLeft(Transform object1)
    {
        if (object1.GetComponent<ILiftable>() != null)
        {
            object1.parent = null;
        }
    }

    private IEnumerator Elevate(float high)
    {
        float sign = Mathf.Sign(high - transform.position.y);
        int floor = CurrentFloor;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        while (true)
        {
            transform.position += Vector3.up * (sign * Time.deltaTime * Speed);

            floor = (int)((transform.position.y - 0.5f) / 5);
            if(floor != CurrentFloor)
            {
                CurrentFloor = floor;

                foreach(ILiftable liftable in Objects)
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

        yield return new WaitForSeconds(5);

        Order = StaticTools.ReduceMassive(Order, 0);

        if(Order.Length > 0)
        {
            ElevateCoroutine = StartCoroutine(Elevate(Order[0] * 5 + 0.5f));
        }
        else
        {
            ElevateCoroutine = null;
        }
    }
}
