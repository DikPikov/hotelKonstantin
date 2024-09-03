using System.Collections;
using UnityEngine;

public class AlcoholObject : ItemObject
{
    private bool Drinking = false;

    public override void Use()
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

        Player._Sanity += 0.25f;

        Player.GetComponentInChildren<FOVEffect>().SetEffect(60, 3, 30);
        FindObjectOfType<LensDistortionEffect>().SetEffect(60, 3, 30);

        Player.ApplyItem(Item, true);

        yield return new WaitForSeconds(1);
        Player._CurrentItem = null;
    }
}
