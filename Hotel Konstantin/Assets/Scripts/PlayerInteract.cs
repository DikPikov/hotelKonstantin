using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject InteractMarker;
    [SerializeField] private RectTransform[] Skobi;

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
        if (Physics.Raycast(Camera.transform.position, Camera.forward, out hit, Distance))
        {
            IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                InteractMarker.SetActive(true);

                if(Interacting == null)
                {
                    Skobi[0].anchoredPosition = new Vector2(-50, 0);
                    Skobi[1].anchoredPosition = new Vector2(50, 0);

                    if (InputManager.GetButtonDown(InputManager.ButtonEnum.Interact))
                    {
                        Interacting = StartCoroutine(Interact(interactable));
                    }
                }
            }
            else
            {
                if (Interacting != null)
                {
                    StopCoroutine(Interacting);
                    Interacting = null;
                }

                InteractMarker.SetActive(false);
            }
        }
        else
        {
            if(Interacting != null)
            {
                StopCoroutine(Interacting);
                Interacting = null;
            }

            InteractMarker.SetActive(false);
        }
    }

    private IEnumerator Interact(IInteractable interactable)
    {
        float timer = interactable._BeforeTime;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while (InputManager.GetButton(InputManager.ButtonEnum.Interact))
        {
            timer -= Time.deltaTime;

            Skobi[0].anchoredPosition = new Vector2(-10 - 40 * (timer/ interactable._BeforeTime), 0);
            Skobi[1].anchoredPosition = new Vector2(10 + 40 * (timer / interactable._BeforeTime), 0);

            if (timer <= 0)
            {
                interactable.Interact();

                Interacting = null;
                yield break;
            }

            yield return waitForEndOfFrame;
        }

        Interacting = null;
    }
}
