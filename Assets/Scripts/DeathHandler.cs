using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class DeathHandler : MonoBehaviour {

    [SerializeField] private Canvas gameOverCanvas;

    private void Start() {
        gameOverCanvas.enabled = false;
        FindObjectOfType<FirstPersonController>().enabled = true;
        Time.timeScale = 1;
        Cursor.visible = false;
        
        
    }

    public void HandleDeath() {
        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        FindObjectOfType<FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
}
