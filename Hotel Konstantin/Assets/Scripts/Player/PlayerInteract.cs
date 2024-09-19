using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject InteractButton;

    [SerializeField] private GameObject InteractMarker;
    [SerializeField] private RectTransform[] Skobi;

    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private Player Player;
    [SerializeField] private Transform Camera;
    [SerializeField] private float Distance;

    private Coroutine Interacting = null;
    private IInteractable CurrentInteractable;

    private void Update()
    {
        if (Pause._Paused || Game._GameOver)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.transform.position, Camera.forward, out hit, Distance, LayerMask))
        {
            IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

            if (interactable != null && interactable._CanInteract)
            {
                if(CurrentInteractable != interactable)
                {
                    if (CurrentInteractable is IAnimated)
                    {
                        (CurrentInteractable as IAnimated).Animate(false);
                    }

                    if (Interacting != null)
                    {
                        StopCoroutine(Interacting);
                        Interacting = null;
                    }
                }

                CurrentInteractable = interactable;

                InteractButton.SetActive(true);
                InteractMarker.SetActive(true);

                if(Interacting == null)
                {
                    Skobi[0].anchoredPosition = new Vector2(-50, 50);
                    Skobi[1].anchoredPosition = new Vector2(50, 50);
                    Skobi[2].anchoredPosition = new Vector2(50, -50);
                    Skobi[3].anchoredPosition = new Vector2(-50, -50);

                    if (InputManager.GetButtonDown(InputManager.ButtonEnum.Interact))
                    {
                        Interact(true);
                    }
                }
            }
            else
            {
                Deinteract();
            }
        }
        else
        {

            foreach(Collider collider in Physics.OverlapSphere(Camera.transform.position, 0.25f, LayerMask))
            {
                IInteractable interactable = collider.GetComponentInParent<IInteractable>();

                if(interactable != null)
                {
                    if (CurrentInteractable != interactable)
                    {
                        if (CurrentInteractable is IAnimated)
                        {
                            (CurrentInteractable as IAnimated).Animate(false);
                        }

                        if (Interacting != null)
                        {
                            StopCoroutine(Interacting);
                            Interacting = null;
                        }
                    }

                    if (interactable._CanInteract)
                    {
                        CurrentInteractable = interactable;

                        InteractButton.SetActive(true);
                        InteractMarker.SetActive(true);

                        if (Interacting == null)
                        {
                            Skobi[0].anchoredPosition = new Vector2(-50, 50);
                            Skobi[1].anchoredPosition = new Vector2(50, 50);
                            Skobi[2].anchoredPosition = new Vector2(50, -50);
                            Skobi[3].anchoredPosition = new Vector2(-50, -50);

                            if (InputManager.GetButtonDown(InputManager.ButtonEnum.Interact))
                            {
                                Interact(true);
                            }
                        }
                    }
                    else
                    {
                        Deinteract();
                    }
                    return;
                }
            }

            Deinteract();
        }
    }

    private void Deinteract()
    {   
        if(CurrentInteractable != null)
        {
            if (CurrentInteractable is IAnimated)
            {
                (CurrentInteractable as IAnimated).Animate(false);
            }

            CurrentInteractable = null;
        }

        if (Interacting != null)
        {
            StopCoroutine(Interacting);
            Interacting = null;
        }

        InteractMarker.SetActive(false);

        if (Player._CurrentItem != null && Player._CurrentItem._Interactable)
        {
            InteractButton.SetActive(true);

            if (InputManager.GetButtonDown(InputManager.ButtonEnum.Interact))
            {
                Player._CurrentItem.Use(true);
            }
        }
        else
        {
            InteractButton.SetActive(false);
        }
    }

    public void Interact(bool state)
    {
        if (state)
        {
            if(CurrentInteractable == null)
            {
                if(Player._CurrentItem != null)
                {
                    Player._CurrentItem.Use(true);
                }
                return;
            }

            if(Interacting == null)
            {
                Interacting = StartCoroutine(Interact());
            }
        }
        else
        {
            if (Player._CurrentItem != null)
            {
                Player._CurrentItem.Use(false);
            }

            Deinteract();
        }
    }

    private IEnumerator Interact()
    {
        float timer = CurrentInteractable._BeforeTime;
        float time = CurrentInteractable._BeforeTime;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        if(CurrentInteractable is IAnimated)
        {
            (CurrentInteractable as IAnimated).Animate(true);
        }

#if UNITY_ANDROID
        while (CurrentInteractable != null)
#else
        while (InputManager.GetButton(InputManager.ButtonEnum.Interact))
#endif
        {
            timer -= Time.deltaTime;

            Skobi[0].anchoredPosition = new Vector2(-7 - 43 * (timer/ time), 7 + 43 * (timer / time));
            Skobi[1].anchoredPosition = new Vector2(7 + 43 * (timer / time), 7 + 43 * (timer / time));
            Skobi[2].anchoredPosition = new Vector2(7 + 43 * (timer / time), -7 - 43 * (timer / time));
            Skobi[3].anchoredPosition = new Vector2(-7 - 43 * (timer / time), -7 - 43 * (timer / time));
           

            if (timer <= 0)
            {
                CurrentInteractable.Interact();

                if (CurrentInteractable is IAnimated)
                {
                    (CurrentInteractable as IAnimated).Animate(false);
                }

                Interacting = null;
                yield break;
            }

            yield return waitForEndOfFrame;
        }

        if (CurrentInteractable is IAnimated)
        {
            (CurrentInteractable as IAnimated).Animate(false);
        }

        Interacting = null;
    }
}
