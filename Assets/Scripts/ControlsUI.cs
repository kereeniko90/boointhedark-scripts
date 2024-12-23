using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class ControlsUI : MonoBehaviour
{   
    [SerializeField] private GameObject controlUI;
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private WeaponSwitcher weaponSwitcher;
    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            controlUI.SetActive(true);
            Time.timeScale = 0;
            weaponSwitcher.enabled = false;
            firstPersonController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ExitControl() {
        controlUI.SetActive(false);
        Time.timeScale = 1;
        weaponSwitcher.enabled = true;
        firstPersonController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
