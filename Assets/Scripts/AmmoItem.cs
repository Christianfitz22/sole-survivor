using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    public int ammoCount;

    public AudioClip pickupSFX;

    public int PickUp()
    {
        AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
        Destroy(gameObject, 0.5f);
        gameObject.SetActive(false);
        return ammoCount;
    }
}
