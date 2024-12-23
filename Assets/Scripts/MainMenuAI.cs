using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainMenuAI : MonoBehaviour
{
    
    [SerializeField] private float wanderRadius = 10f;

    [SerializeField] private float wanderTimer = 5f;
    

    

    
    

    private float timer;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    

    

    private EnemyAttack enemyAttack;
    

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        

        
        
    }

    

    private void Update() {
        
        Wander();
            
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
            wanderTimer = UnityEngine.Random.Range(1f,5f);
            timer = 0;
        }

        
    }

    private bool IsPositionValidOnNavMesh(Vector3 position) {
        NavMeshHit hit;
        
        return NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas);
}

    
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randomPosition;
        NavMeshHit navHit;

        
        for (int i = 0; i < 30; i++) { 
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist; 
            randDirection += origin; 

        
        if (NavMesh.SamplePosition(randDirection, out navHit, dist, layermask)) {
            randomPosition = navHit.position;
            
            return randomPosition; 
        }
        }

        
        return origin; 
    }

    

    
}
