using UnityEngine;
using System.Collections;

public class Bed : MonoBehaviour, IAnimated
{
    [SerializeField] private Room Room;
    [SerializeField] private Animator Animator;
    [SerializeField] private float Timer;
    [SerializeField] private bool Cleared; 

    public float _BeforeTime => Timer;
    public bool _CanInteract => !Cleared;

    public bool _Cleared
    {
        get
        {
            return Cleared;
        }
        set
        {
            Cleared = value;

            Room.UpdateTaskInfo();

            Animator.SetBool("Clear", Cleared);

            if (Cleared)
            {
                Animator.Play("Cleared");
            }
        }
    }

    public void Interact()
    {
        _Cleared = !Cleared;
        Animator.Play("Cleared");
    }

    public void Animate(bool state)
    {
        Animator.SetFloat("Clearing", state.GetHashCode());

        StopAllCoroutines();

        if (state)
        {
            StartCoroutine(DeTime());
        }
    }

    private IEnumerator DeTime()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while(Timer > 0)
        {
            Timer -= Time.deltaTime;

            yield return waitForEndOfFrame;
        }
    }
}
