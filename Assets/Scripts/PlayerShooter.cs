using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    public Text ammoDisplay;

    public AudioClip pistolShot;
    public AudioClip pistolEmpty;
    public AudioClip pistolReload;

    public ParticleSystem muzzleFlash;

    public Transform cameraTransform;

    public int maxClipSize;

    public int startingBullets;

    public int startingLoadedBullets;

    private int stockedBullets;

    private int loadedBullets;

    // Start is called before the first frame update
    void Start()
    {
        stockedBullets = startingBullets;
        loadedBullets = startingLoadedBullets;
        DisplayAmmo();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (loadedBullets > 0)
            {
                ShootPistol();
            }
            else
            {
                AudioSource.PlayClipAtPoint(pistolEmpty, transform.position);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (stockedBullets > 0 && loadedBullets < maxClipSize)
            {
                int ammoTransfer = System.Math.Min(stockedBullets, maxClipSize - loadedBullets);
                stockedBullets -= ammoTransfer;
                loadedBullets += ammoTransfer;
                DisplayAmmo();
                AudioSource.PlayClipAtPoint(pistolReload, transform.position);
            }
            else if (stockedBullets == 0)
            {
                AudioSource.PlayClipAtPoint(pistolEmpty, transform.position);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CollectItem();
        }
    }

    void DisplayAmmo()
    {
        ammoDisplay.text = "Ammo\n" + loadedBullets + "/" + stockedBullets;
    }

    void ShootPistol()
    {
        loadedBullets -= 1;
        DisplayAmmo();

        AudioSource.PlayClipAtPoint(pistolShot, transform.position);
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.transform.gameObject.name);
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.transform.gameObject.GetComponent<EnemyHits>().HitFor(1);
            }
        }
    }

    void CollectItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Ammo"))
            {
                int ammoAdd = hit.transform.gameObject.GetComponent<AmmoItem>().PickUp();
                stockedBullets += ammoAdd;
                DisplayAmmo();
            }
        }
    }
}