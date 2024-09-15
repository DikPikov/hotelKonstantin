using System.Collections;
using UnityEngine;

public class AlcoholObject : ItemObject
{
    private bool Drinking = false;

    public override void Use(bool state)
    {
        if (!Drinking)
        {
            Drinking = true;

            StartCoroutine(Drink());
        }
    }

    private IEnumerator Drink()
    {
        Animator.SetBool("Drink", true);

        yield return new WaitForSeconds(4);

        Player.GetComponentInChildren<FOVEffect>().SetEffect(60, 3, 30);
        FindObjectOfType<LensDistortionEffect>().SetEffect(60, 3, 30);

        Player.ApplyItem(Item, true);
    }
}
