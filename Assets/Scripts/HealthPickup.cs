using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healthAmount = 50f;
    

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "Player") {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            PlayerSound playerSound = other.gameObject.GetComponent<PlayerSound>();
            playerSound.PlayHealthPickup();
            playerHealth.IncreaseHealth(healthAmount);
            Destroy(gameObject);
        }
    }
}
