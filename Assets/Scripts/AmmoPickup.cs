using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {

    [SerializeField] int ammoAmount = 5;
    [SerializeField] AmmoType ammoType;

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "Player") {
            Ammo ammo = other.gameObject.GetComponent<Ammo>();
            PlayerSound playerSound = other.gameObject.GetComponent<PlayerSound>();
            playerSound.PlayAmmoPickup();
            ammo.IncreaseCurrentAmmo(ammoType, ammoAmount);
            Destroy(gameObject);
        }
    }

}
