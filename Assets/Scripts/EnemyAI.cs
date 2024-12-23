using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    
    [SerializeField] private float targetRange = 5f;
    [SerializeField] private float wanderRadius = 5f;

    [SerializeField] private float wanderTimer = 5f;
    [SerializeField] private float turnSpeed = 10f;

    [SerializeField] GameObject dieEffect;

    private EnemyHealth enemyHealth;

    private Transform targetPlayer;

    

    private float timer;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float timeToDestroy = 2f;

    float deathTimer = 0f;

    float distanceToTarget = Mathf.Infinity;
    private bool isProvoked;

    

    private EnemyAttack enemyAttack;
    

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        enemyHealth = GetComponent<EnemyHealth>();
        targetPlayer = GameObject.Find("pfPlayer").GetComponent<Transform>();

        enemyHealth.OnDamageTaken += enemyHealth_OnDamageTaken;
        
    }

    private void enemyHealth_OnDamageTaken(object sender, EventArgs e)
    {
        
        isProvoked = true;
        
        
    }

    private void Update() {

        

        distanceToTarget = Vector3.Distance(targetPlayer.position, transform.position);

        if (!enemyHealth.IsDead()) {
            if (isProvoked) {
            EngageTarget();
            } else if (distanceToTarget <= targetRange) {
            
                isProvoked = true;
                        
            } else if (!isProvoked) {
            Wander();
            }

        } else {
            navMeshAgent.SetDestination(transform.position);
            
            
            
            deathTimer += Time.deltaTime;

            

            if (deathTimer > timeToDestroy) {
                Vector3 particlePosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                Instantiate(dieEffect, particlePosition, Quaternion.identity);
                
                Destroy(gameObject);
            }
        }
        
        
        

        
    }

    private void EngageTarget() {

        FaceTarget();

        if (distanceToTarget > navMeshAgent.stoppingDistance) {
            ChaseTarget();
        }


        if (distanceToTarget <= navMeshAgent.stoppingDistance + 0.5f) {
            AttackTarget();
        }  
    }

    private void ChaseTarget() {

        
        animator.SetBool("isMoving", true);
        animator.SetBool("attack", false);
        
        navMeshAgent.SetDestination(targetPlayer.transform.position);
    }

    private void AttackTarget() {

        

        animator.SetBool("attack", true);
        
    }

    

    private void FaceTarget() {

        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void Wander() {
        timer += Time.deltaTime;
        
        if (navMeshAgent.velocity.magnitude > 0.1f) {
        animator.SetBool("isMoving", true);  // Set moving animation
        } else {
        animator.SetBool("isMoving", false); // Set idle animation
        }
        

        if (timer >= wanderTimer) {
            
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);

            // Check if the NavMeshAgent is active and valid before setting the destination
            if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled) {
            // Check if the new position is valid on the NavMesh
                if (IsPositionValidOnNavMesh(newPos)) {
                    navMeshAgent.SetDestination(newPos);
                } else {
                    Debug.LogWarning("Random position is not valid on the NavMesh.");
                }
            } else {
                Debug.LogWarning("NavMeshAgent is not active or has not been initialized properly.");
            }
            wanderTimer = UnityEngine.Random.Range(5f,10f);
            timer = 0;
        }

        
    }

    private bool IsPositionValidOnNavMesh(Vector3 position) {
        NavMeshHit hit;
        // Check if the position is on the NavMesh
        return NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas);
}

    
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randomPosition;
        NavMeshHit navHit;

        // Keep trying until we find a valid position
        for (int i = 0; i < 30; i++) { // Limit attempts to prevent infinite loops
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist; // Generate random direction
            randDirection += origin; // Add origin to get a position relative to the origin

        // Sample the NavMesh to see if the position is valid
        if (NavMesh.SamplePosition(randDirection, out navHit, dist, layermask)) {
            randomPosition = navHit.position;
            
            return randomPosition; 
        }
        }

        // If no valid position is found after 10 attempts, return the original position
        Debug.LogWarning("Could not find a valid position on the NavMesh after 10 attempts.");
        return origin; // Return the origin if no valid position is found
    }

    private void OnDrawGizmosSelected() {
                
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRange);
    }

    

    
}
