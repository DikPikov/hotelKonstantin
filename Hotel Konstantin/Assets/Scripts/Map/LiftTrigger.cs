using UnityEngine;

public class LiftTrigger : MonoBehaviour
{
    [SerializeField] private Lift Lift;

    private void OnTriggerEnter(Collider other)
    {
        Lift.ObjectEntered(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        Lift.ObjectLeft(other.transform);
    }
}
