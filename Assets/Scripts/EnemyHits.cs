using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHits : MonoBehaviour
{
    public int hitpoints;

    public AudioClip hitSFX;
    public AudioClip deathSFX;

    public void HitFor(int damage)
    {
        if (damage > 0)
        {
            hitpoints -= damage;
            if (hitpoints <= 0)
            {
                AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
                Destroy(gameObject, 0.5f);
                gameObject.SetActive(false);
            } else
            {
                AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position);
            }
        }
    }
}
