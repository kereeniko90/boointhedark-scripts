using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    
    [SerializeField] private TextMeshProUGUI hpText;

    private RectTransform healthBar;
    private float currentHealth;
    private float targetHealthBarScale;

    

    private float lerpSpeed = 2f;


    private void Start() {
        healthBar = GetComponent<RectTransform>();
    }

    private void Update() {

        
        currentHealth = playerHealth.GetHealth();

        hpText.text = playerHealth.GetHealth().ToString("F0") + "%";

        // Calculate the target health bar scale based on the player's current health

        if (currentHealth > 0) {
            targetHealthBarScale = Mathf.Max(0f, currentHealth / playerHealth.maxHealth * 5);
            hpText.text = playerHealth.GetHealth().ToString("F0") + "%";
        } else {
            targetHealthBarScale = 0f;
            healthBar.transform.localScale = new Vector3(0f, 1.5f, 1);
            hpText.text = "0%";
            return;
        }
        

        // Smoothly transition the health bar scale using Lerp
        float currentBarScale = healthBar.transform.localScale.x;
        float newBarScale = Mathf.Lerp(currentBarScale, targetHealthBarScale, Time.deltaTime * lerpSpeed);

        // Update the health bar scale with the new value
        healthBar.transform.localScale = new Vector3(newBarScale, 1.5f, 1);

        
    }
}
