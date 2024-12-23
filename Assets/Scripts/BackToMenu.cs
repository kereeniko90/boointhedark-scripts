using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{   
    [SerializeField] GameSceneManager gameSceneManager;

    [SerializeField] BossAI bossAI;
    
    private float waitTimer = 0f;
    private float waitTimerMax = 10f;

    private bool startTimer = false;

    private void Start() {
        bossAI.OnDeath += BossAI_OnDeath;
    }

    private void BossAI_OnDeath(object sender, EventArgs e)
    {
        if (!startTimer) {
            startTimer = true;
            Debug.Log("start timer is true");
        }
        
    }

    private void Update() {
        
        if (startTimer) {
            waitTimer += Time.deltaTime;
            
            if (waitTimer >= waitTimerMax) {
                gameSceneManager.MainMenu();
        }
        }
            

        
    }

}

