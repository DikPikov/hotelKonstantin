using UnityEngine;

public class Cosmare : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private Renderer Renderer;
    [SerializeField] private GameObject CosmareNoise;
    [SerializeField] private LayerMask WallLayer;
    private bool Visioned = false;

    public void SetInfo(Player player, GameObject noise)
    {
        Player = player;
        CosmareNoise = noise;
    }

    private void OnDestroy()
    {
        Visioned = true;
        CosmareNoise.SetActive(false);
    }

    private void Update()
    {
        transform.localEulerAngles = new Vector3(0, Quaternion.LookRotation(Player.transform.position - transform.position).eulerAngles.y, 0);

        if (Renderer.isVisible)
        {
            Transform camera = Camera.main.transform;
            if (Physics.Raycast(camera.position, transform.position + new Vector3(0, 0.5f, 0) - camera.position, Vector3.Distance(camera.position, transform.position + new Vector3(0, 0.5f, 0)), WallLayer))
            {
                if (Visioned)
                {
                    Destroy(gameObject);
                }
                return;
            }

            if (!Visioned)
            {
                CosmareNoise.SetActive(true);

                Player._Sanity -= 0.1f;
                Visioned = true;
            }

            Player._Sanity -= Time.deltaTime * 0.25f;

            if (Player._Sanity == 0)
            {
                Game.Over();
                Destroy(gameObject);
            }
        }
        else if (Visioned)
        {
            Destroy(gameObject);
        }
    }
}
