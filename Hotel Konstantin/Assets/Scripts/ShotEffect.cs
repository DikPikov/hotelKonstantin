using UnityEngine;

public class ShotEffect : MonoBehaviour
{
    [SerializeField] private Light Light;
    [SerializeField] private float FadeSpeed;

    private void Start()
    {
        PlayerRotation rotation = FindObjectOfType<PlayerRotation>();

        rotation._Rotation = new Vector3(rotation._Rotation.x + Random.Range(-15, -5f), rotation._Rotation.y + Random.Range(-5, 5f), rotation._Rotation.z);
    }

    private void Update()
    {
        Light.range -= FadeSpeed * Time.deltaTime;
        Light.intensity -= FadeSpeed * Time.deltaTime / 4f;
    }
}
