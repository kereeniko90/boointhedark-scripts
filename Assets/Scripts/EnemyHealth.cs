using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] float hitPoints = 100f;

    private bool isDead = false;

    private float maxHp;
    public bool IsDead() {
        return isDead;
    }


    public event EventHandler OnDamageTaken;

    private EnemyAttack enemyAttack;

    private Animator animator;
    private EnemyAI enemyAI;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        
        maxHp = hitPoints;
    }

    public void TakeDamage(float damage) {

        OnDamageTaken?.Invoke(this, EventArgs.Empty);
        hitPoints -= damage;
        


        if (hitPoints <= 0) {
            Die();
        }
    }

    private void Die()
    {   
        if (isDead) return;
        isDead = true;

        if (enemyAI != null) {
            animator.SetTrigger("die");
        }
        
    }

    public float GetCurrentHealthPercentage() {
        return hitPoints / maxHp * 100;
    }
}
