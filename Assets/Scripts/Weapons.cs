using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapons : MonoBehaviour {
    
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Camera weaponCamera;
    [SerializeField] private float range = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private Ammo ammoSlot;
    [SerializeField] private GameObject rayPosition;
    [SerializeField] private float fireRate = 1;
    [SerializeField] private float hitForce = 1f;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI gunName;

    [SerializeField] private Image weapon1Image;
    [SerializeField] private Image weapon2Image;
    [SerializeField] private Image weapon3Image;
    [SerializeField] AmmoType ammoType;

    [SerializeField] private int maxAmmo = 20;

    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;

    [SerializeField] private LayerMask ignoreLayer;

    private Animator animator;

    private AudioSource audioSource;

    private bool canShoot = true;
    private bool isReloading = false;

    private int currentAmmo;

    private void Start() {
        
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ammoSlot.DecreaseAmmoReload(ammoType,maxAmmo);
        currentAmmo = maxAmmo;
        DisplayGunInfo();

    }

    private void OnEnable() {
        DisplayGunInfo();
        canShoot = true;
        isReloading = false;
        animator = GetComponent<Animator>();
        animator.SetBool("reloading", false);

    }
    private void Update() {

        DisplayGunInfo();
        
        if (Input.GetButton("Fire1") && canShoot && currentAmmo > 0) {

            StartCoroutine(Shoot());
                
        }

        if (currentAmmo <= 0 && ammoSlot.GetCurrentAmmo(ammoType) > 0 && !isReloading) {
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo != maxAmmo) {
            StartCoroutine(Reload());
        }
        

        
    }

    private IEnumerator Reload()
    {
        canShoot = false;
        isReloading = true;
        animator.SetBool("reloading", true);
        audioSource.PlayOneShot(reloadSound, audioSource.volume);

        
        yield return new WaitForSeconds(reloadTime);

        int ammoAvailable = ammoSlot.GetCurrentAmmo(ammoType);

        
        int ammoToReload = Mathf.Min(maxAmmo - currentAmmo, ammoAvailable);

        if (ammoAvailable > 0 && ammoToReload > 0) {
            currentAmmo += ammoToReload;
            ammoSlot.DecreaseAmmoReload(ammoType, ammoToReload);
        }
        

        canShoot = true;
        isReloading = false;
        animator.SetBool("reloading", false);
    }

    IEnumerator Shoot()
    {   

        canShoot = false;
        if (currentAmmo > 0) {
            PlayMuzzleFlash();
            currentAmmo--;
            audioSource.PlayOneShot(shootSound, audioSource.volume);
            ProcessRayCast();
            //ammoSlot.DecreaseCurrentAmmo(ammoType);
                
        }
        yield return new WaitForSeconds(fireRate);
        canShoot = true;

    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range, ~ignoreLayer))
        {
            
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            CreateWeaponTracers(rayPosition.transform.position, hit.point);
            
            if (target == null) return;
            target.TakeDamage(damage);

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
            
        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        impact.transform.parent = hit.transform;
        
        Destroy(impact, 1);
    }

    private void CreateWeaponTracers (Vector3 fromPosition, Vector3 targetPosition) {
        Vector3 dir = (targetPosition - fromPosition).normalized;
    }

    private void DisplayGunInfo() {

        int totalAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        string currentGun = ammoSlot.GetGunName(ammoType);
        int gunIndex = ammoSlot.GetGunIndex(ammoType);
        ammoText.text = currentAmmo + "/" + totalAmmo; 
        gunName.text = currentGun;

        
        GameObject[] weaponImages = { weapon1Image.gameObject, weapon2Image.gameObject, weapon3Image.gameObject};

    
        for (int i = 0; i < weaponImages.Length; i++) {

            weaponImages[i].SetActive(i == gunIndex);
        }

    
     }
}
