using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    [SerializeField] private Room Room;

    public float _BeforeTime => Random.Range(0.3f, 1f);
    public bool _CanInteract => true;

    public void Interact()
    {
        Room.DeleteTrash(gameObject);
        Room.UpdateTaskInfo();
        Destroy(gameObject);
    }
}
