using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private AudioSource heartbeatSound;

    public float maxHealth = 100f;

    private DeathHandler deathHandler;

    public event EventHandler OnDamageTaken;
    public event EventHandler OnDeath;


    private bool isDead = false;
    private bool startHeartbeat = false;

    private void Start() {
        deathHandler = GetComponent<DeathHandler>();
    }


    public void TakeDamage(float damage) {

        health -= damage;
        //CinemachineShake.Instance.ShakeCamera(5f, 0.1f);
        OnDamageTaken?.Invoke(this, EventArgs.Empty);
        RedVignetteDamage.Instance.SetWeight(1f);

    }

    private void Update() {
        if (health <= 0) {

            if (!isDead) {
                OnDeath?.Invoke(this, EventArgs.Empty);
                isDead = true;
            }
            
            deathHandler.HandleDeath();
            //
        } 
        
        if (health > 0f && health <= 30f && !startHeartbeat) {

            if (!heartbeatSound.isPlaying) {
                heartbeatSound.gameObject.SetActive(true); // or heartbeatSound.Play();
                startHeartbeat = true;
            }
        } else if (health > 30f && startHeartbeat) {
            heartbeatSound.gameObject.SetActive(false); // or heartbeatSound.Stop();
            startHeartbeat = false;
        }
    }

    public float GetHealth() {
        return health;
    }

    public void IncreaseHealth(float healthAmount) {
        health += healthAmount;
        if (health > 100f) {
            health = 100f;
        }
    }
}
