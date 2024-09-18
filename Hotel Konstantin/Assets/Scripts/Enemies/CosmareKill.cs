using UnityEngine;
using UnityEngine.UI;

public class CosmareKill : MonoBehaviour
{
    [SerializeField] private Image Image;

    public void Active()
    {
        Image.color = new Color(Random.value, Random.value, Random.value);
        Image.gameObject.SetActive(true);
    }
}