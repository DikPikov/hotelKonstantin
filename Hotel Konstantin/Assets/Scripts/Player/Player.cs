using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ILiftable
{
    [SerializeField] private Floor Floor;
    [SerializeField] private Lift Lift;

    [SerializeField] private AudioSource LightDistorting;
    [SerializeField] private Transform Camera;

    [SerializeField] private float Stamina;

    private ItemObject CurrentItem = null;
    private Item[] Items = new Item[0];

    public event SimpleVoid OnFloorChange = null;
    public event SimpleVoid OnChanges = null;
    public event SimpleVoid OnItemChanges = null;

    public Transform _Transform => transform;
    public ItemObject _CurrentItem
    {
        get
        {
            return CurrentItem;
        }
        set
        {
            if(CurrentItem != null && CurrentItem != value)
            {
                Destroy(CurrentItem.gameObject);
            }

            CurrentItem = value;

            if (OnItemChanges != null) 
            {
                OnItemChanges.Invoke();
            }
        }
    }
    public Item[] _Items => Items;

    public Lift _Lift
    {
        get
        {
            return Lift;
        }
        set
        {
            Lift = value;
        }
           
    }
    public Floor _Floor
    {
        get 
        { 
            return Floor;
        }
        set
        {
            if(Floor == value)
            {
                return;
            }

            //if (Floor != null)
            //{
            //    Floor.gameObject.SetActive(false);
            //}

            Floor = value;

            if(GameMap._MapLights.GetFloorState(Floor) == 1)
            {
                LightDistorting.volume = 1;
            }
            else
            {
                LightDistorting.volume = 0;
            }
            //if (Floor != null)
            //{
            //    Floor.gameObject.SetActive(true);
            //}

            if (OnFloorChange != null)
            {
                OnFloorChange.Invoke();
            }
        }
   }

    public float _Stamina
    {
        get
        {
            return Stamina;
        }
        set
        {
            Stamina = Mathf.Clamp(value, 0, 5);

            if (OnChanges != null)
            {
                OnChanges.Invoke();
            }
        }
    }

    public void AnimateDeath(bool state)
    {
        StopAllCoroutines();

        if (state)
        {
            Camera.GetComponent<Camera>().fieldOfView = 60;

            StartCoroutine(SetCameraPosition(new Vector3(0, 0.2f, 0), 90 * ((Random.value > 0.5f) ? -1 : 1), true));
        }
        else
        {
            Camera.GetComponent<Camera>().fieldOfView = 75;

            StartCoroutine(SetCameraPosition(new Vector3(0, 1.7f, 0), 0, false));
        }
    }

    public bool ApplyItem(Item item, bool remove)
    {
        int index = StaticTools.IndexOf(Items, item);

        if (remove)
        {
            if(index < 0)
            {
                return false;
            }
            else
            {
                if(CurrentItem._Item == Items[index])
                {
                    _CurrentItem = null;
                }

                Items = StaticTools.ReduceMassive(Items, index);
            }
        }
        else
        {
            if(Items.Length >= 3)
            {
                return false;
            }

            if(index < 0)
            {
                Items = StaticTools.ExcludingExpandMassive(Items, item);
            }
            else
            {
                return false;
            }
        }

        if(OnItemChanges != null)
        {
            OnItemChanges.Invoke();
        }

        return true;
    }

    private IEnumerator SetCameraPosition(Vector3 localPosition, float zTarget, bool showGameOverPanel)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        float positionDistance = Vector3.Distance(localPosition, Camera.localPosition);

        float zRotation = 0;
        float rotationDistance = Mathf.Abs(zRotation - zTarget);

        while (positionDistance > 0 || rotationDistance > 0)
        {
            if(positionDistance > 0)
            {
                Camera.localPosition += (localPosition - Camera.localPosition) * Time.deltaTime * 1.5f / positionDistance;

                float newDistance = Vector3.Distance(localPosition, Camera.localPosition);
                if (newDistance > positionDistance)
                {
                    newDistance = 0;
                    Camera.localPosition = localPosition;
                }

                positionDistance = newDistance;
            }

            if (rotationDistance > 0)
            {
                zRotation += (zTarget - zRotation) * Time.deltaTime * 90f / rotationDistance;
                Camera.localEulerAngles = new Vector3(0, 0, zRotation);

                float newDistance = Mathf.Abs(zTarget - zRotation);
                if (newDistance > rotationDistance)
                {
                    newDistance = 0;
                    Camera.localEulerAngles = new Vector3(0, 0, zTarget);
                }

                rotationDistance = newDistance;
            }
            yield return waitForEndOfFrame;
        }

        if (showGameOverPanel)
        {
            Game.Over();
        }
    }
}
