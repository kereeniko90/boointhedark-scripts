using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour {

    [SerializeField] AmmoSlot[] ammoSlots;

    
    
    [System.Serializable]
    private class AmmoSlot {
        public AmmoType ammoType;
        public int ammoAmount;

        public string gunName;

        public int gunIndex;

        public void SetAmmoAmount(int amount) {
            ammoAmount = amount;
        }
    }


    public int GetCurrentAmmo(AmmoType ammoType) {
        return GetAmmoSlot(ammoType).ammoAmount;
    }

    public string GetGunName(AmmoType ammoType) {
        return GetAmmoSlot(ammoType).gunName;
    }

    public int GetGunIndex(AmmoType ammoType) {
        return GetAmmoSlot(ammoType).gunIndex;
    }

    public void DecreaseCurrentAmmo (AmmoType ammoType) {
        GetAmmoSlot(ammoType).ammoAmount--;
    }

    public void DecreaseAmmoReload(AmmoType ammoType, int ammoReload) {
        var ammoSlot = GetAmmoSlot(ammoType);

        if (ammoSlot != null) {
            int newAmmoAmount = GetCurrentAmmo(ammoType) - ammoReload;
            
            ammoSlot.SetAmmoAmount(newAmmoAmount);
        }
        
        
    }

    public void IncreaseCurrentAmmo (AmmoType ammoType, int ammoAmount) {
        GetAmmoSlot(ammoType).ammoAmount += ammoAmount;
    }

    private AmmoSlot GetAmmoSlot (AmmoType ammoType) {
        foreach (AmmoSlot slot in ammoSlots) {
            if (slot.ammoType == ammoType)  {
                return slot;
            }
        }
        return null;
    }

    private void SetAmmoAmount(int ammoAmount) {

    }
        
        
    
    
}
