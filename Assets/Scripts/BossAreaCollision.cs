using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossAreaCollision : MonoBehaviour
{
    
    [SerializeField] GameObject bossInfo;
    [SerializeField] TextMeshProUGUI bossHpText;

    [SerializeField] GameObject bossObject;

    [SerializeField] RectTransform bossHpBar;
    [SerializeField] GameMusicManager gameMusicManager;

    [SerializeField] GameObject bossWall;

    private EnemyHealth bossHealth;

    private Animator animator;


    private void Start() {
        bossInfo.SetActive(false);
        if (bossObject != null) {
            bossHealth = bossObject.GetComponent<EnemyHealth>();
            animator = bossObject.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "Player") {
            bossInfo.SetActive(true);
            gameMusicManager.PlayBossMusic();
            bossWall.SetActive(true);
        } 
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            bossInfo.SetActive(false);
            gameMusicManager.PlayBackgroundMusic();
        } 
    }

    private void Update() {
        
        if (bossObject != null) {
            float currentHp = bossHealth.GetCurrentHealthPercentage();
            
            if (currentHp > 0f) {
                bossHpText.text = currentHp.ToString("0") + "%";
                float hpBarX = currentHp / 100f * 1;
                bossHpBar.transform.localScale = new Vector3(hpBarX, bossHpBar.transform.localScale.y,bossHpBar.transform.localScale.z);
            } else {
                bossHpText.text = "0%";
                bossHpBar.transform.localScale = new Vector3(0f, bossHpBar.transform.localScale.y,bossHpBar.transform.localScale.z);
            }            
            
        }
    }


}
