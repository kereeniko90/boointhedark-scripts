using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip[] hurtSounds;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private AudioClip ammoPickup;
    [SerializeField] private AudioClip healthPickup;
    [SerializeField] private AudioClip earthquake;
    [SerializeField] private AudioClip noStamina;
    
    

     
    private AudioSource audioSource;
    private PlayerHealth playerHealth;
    

    private Transform player;
    //private float footstepTimer = 0f;

    private void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();

       playerHealth.OnDamageTaken += PlayerHealth_OnDamageTaken;
       playerHealth.OnDeath += PlayerHealth_OnDeath;
    }

    private void PlayerHealth_OnDeath(object sender, EventArgs e) {
        
        audioSource.PlayOneShot(deathSound, audioSource.volume);
    }

    private void PlayerHealth_OnDamageTaken(object sender, EventArgs e) {   
        int randomNumber = UnityEngine.Random.Range(0, hurtSounds.Length);
        audioSource.PlayOneShot(hurtSounds[randomNumber], audioSource.volume);
    }

    private void Update()
    {
        
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound, audioSource.volume);
             
    }

    public void PlayAmmoPickup() {
        audioSource.PlayOneShot(ammoPickup, audioSource.volume);
    }

    public void PlayHealthPickup() {
        audioSource.PlayOneShot(healthPickup, audioSource.volume);
    }

    public void EarthquakeSound() {
        audioSource.PlayOneShot(earthquake, audioSource.volume);
        
    }

    public void NoStaminaSound() {
        audioSource.PlayOneShot(noStamina, audioSource.volume);
    }

        
}

    

