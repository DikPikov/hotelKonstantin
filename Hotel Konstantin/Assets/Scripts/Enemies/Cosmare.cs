using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Cosmare : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private Image CosmareNoise;
    [SerializeField] private LayerMask WallLayer;
    private bool Visioned = false;

    private Lighter Lighter = null;
    private float Intense = 0;

    private float SanitySpeed = 0.01f;

    private bool Killing = false;

    public void SetInfo(Player player, Image noise)
    {
        Player = player;
        CosmareNoise = noise;

        SanitySpeed = 0.01f + Game._HotelMadness * 0.24f;

        foreach(Collider collider in Physics.OverlapSphere(transform.position, 5))
        {
            if (collider.GetComponent<Lighter>())
            {
                Lighter = collider.GetComponent<Lighter>();
                break;
            }
        }

        if(Lighter != null)
        {
            Intense = Lighter._Intensity;

            Lighter._Intensity = 0;
        }
    }

    private void OnDestroy()
    {
        Visioned = true;
        CosmareNoise.color = new Color(1, 0, 0, 0);

        if (Lighter != null)
        {
            Lighter._Intensity = Intense;
        }
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
                CosmareNoise.color = new Color(1, 0, 0, 1 - Player._Sanity);

                Player._Sanity -= 0.1f;
                Visioned = true;
            }

            Player._Sanity -= Time.deltaTime * 0.05f;
            CosmareNoise.color = new Color(1, 0, 0, 1 - Player._Sanity);

            if (Player._Sanity == 0)
            {
                Killing = true;
                StartCoroutine(Kill());
            }
        }
        else if (Visioned && Game._HotelMadness < 0.5f && !Killing)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Kill()
    {
        CosmareNoise.color = new Color(1, 0, 0, 1);

        yield return new WaitForSecondsRealtime(1);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
