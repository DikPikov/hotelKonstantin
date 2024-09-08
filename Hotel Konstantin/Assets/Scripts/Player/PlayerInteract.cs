using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject InteractMarker;
    [SerializeField] private RectTransform[] Skobi;

    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private Player Player;
    [SerializeField] private Transform Camera;
    [SerializeField] private float Distance;

    private Coroutine Interacting = null;

    private void Update()
    {
        if (Pause._Paused)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.transform.position, Camera.forward, out hit, Distance, LayerMask))
        {
            IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

            if (interactable != null && interactable._CanInteract)
            {
                InteractMarker.SetActive(true);

                if(Interacting == null)
                {
                    Skobi[0].anchoredPosition = new Vector2(-50, 50);
                    Skobi[1].anchoredPosition = new Vector2(50, 50);
                    Skobi[2].anchoredPosition = new Vector2(50, -50);
                    Skobi[3].anchoredPosition = new Vector2(-50, -50);

                    if (InputManager.GetButtonDown(InputManager.ButtonEnum.Interact))
                    {
                        Interacting = StartCoroutine(Interact(interactable));
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
            Deinteract();
        }
    }

    private void Deinteract()
    {
        if (Interacting != null)
        {
            StopCoroutine(Interacting);
            Interacting = null;
        }

        InteractMarker.SetActive(false);

        if (InputManager.GetButtonDown(InputManager.ButtonEnum.Interact))
        {
            if (Player._CurrentItem != null)
            {
                Player._CurrentItem.Use();
            }
        }
    }

    private IEnumerator Interact(IInteractable interactable)
    {
        float timer = interactable._BeforeTime;
        float time = interactable._BeforeTime;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        if(interactable is IAnimated)
        {
            (interactable as IAnimated).Animate(true);
        }

        while (InputManager.GetButton(InputManager.ButtonEnum.Interact))
        {
            timer -= Time.deltaTime;

            Skobi[0].anchoredPosition = new Vector2(-7 - 43 * (timer/ time), 7 + 43 * (timer / time));
            Skobi[1].anchoredPosition = new Vector2(7 + 43 * (timer / time), 7 + 43 * (timer / time));
            Skobi[2].anchoredPosition = new Vector2(7 + 43 * (timer / time), -7 - 43 * (timer / time));
            Skobi[3].anchoredPosition = new Vector2(-7 - 43 * (timer / time), -7 - 43 * (timer / time));
           

            if (timer <= 0)
            {
                interactable.Interact();

                if (interactable is IAnimated)
                {
                    (interactable as IAnimated).Animate(false);
                }

                Interacting = null;
                yield break;
            }

            yield return waitForEndOfFrame;
        }

        if (interactable is IAnimated)
        {
            (interactable as IAnimated).Animate(false);
        }

        Interacting = null;
    }
}
