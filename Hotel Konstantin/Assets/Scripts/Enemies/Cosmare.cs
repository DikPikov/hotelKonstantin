using UnityEngine;

public class Cosmare : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private Transform PlayerCamera;

    private void Update()
    {
        transform.localEulerAngles = new Vector3(0, Quaternion.LookRotation(Player.transform.position - transform.position).eulerAngles.y, 0);

        if (Vector3.Distance(transform.position, Player.transform.position) < 0.25f)
        {
            Player._Sanity -= 0.2f;

            Destroy(gameObject);

            return;
        }

        RaycastHit hit;
        if(Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out hit))
        {
            if(hit.transform == transform)
            {
                Player._Sanity -= 0.2f;

                Destroy(gameObject);
            }
        }
    }
}
