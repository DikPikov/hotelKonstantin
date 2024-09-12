using UnityEngine;

public class Dirt : MonoBehaviour, IInteractable
{
    [SerializeField] private Room Room;

    public float _BeforeTime => Random.Range(0.5f, 1.2f);
    public bool _CanInteract => true;

    public void Interact()
    {
        Room.DeleteTrash(gameObject);
        Room.UpdateTaskInfo();
        Destroy(gameObject);
    }
}
