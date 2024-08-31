using UnityEngine;

public class GhostHead : MonoBehaviour
{
    [SerializeField] private Transform PlayerCamera;

    private void Start()
    {
        PlayerCamera = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(PlayerCamera);
    }
}
