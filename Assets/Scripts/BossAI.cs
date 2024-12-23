using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{

    [SerializeField] private float targetRange = 5f;
    [SerializeField] private float wanderRadius = 5f;

    [SerializeField] private float wanderTimer = 5f;
    [SerializeField] private float turnSpeed = 5f;

    [SerializeField] private Canvas winGameCanvas;
    [SerializeField] private GameObject checkmark;

    public event EventHandler OnDeath;


    [SerializeField]private float wanderingSpeed = 3.5f;

    [SerializeField]private float runningSpeed = 11.5f;

    [SerializeField]private float rageSpeed = 13.5f;


    private EnemyHealth enemyHealth;

    private PlayerHealth playerHealth;

    private Transform targetPlayer;

    private PlayerSound playerSound;

    private CapsuleCollider bossCollider;

    private float timer;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float timeToDestroy = 2f;

    private float deathTimer = 0f;

    private Vector3 originalPos;

    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked;
    private bool isDead = false;


    private bool isHowling = false;
    private bool howlingTriggered = false;

    private float currentHp;

    private float winTimerMax = 10f;
    private float winTimer = 0f;
    private bool earthquakeSound = false;


    private void Start()
    {
        winGameCanvas.enabled = false;
        originalPos = transform.position;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        bossCollider = GetComponent<CapsuleCollider>();
        targetPlayer = GameObject.Find("pfPlayer").GetComponent<Transform>();
        playerHealth = GameObject.Find("pfPlayer").GetComponent<PlayerHealth>();
        playerSound = targetPlayer.GetComponent<PlayerSound>();

        enemyHealth.OnDamageTaken += enemyHealth_OnDamageTaken;
        navMeshAgent.speed = wanderingSpeed;

    }

    private void enemyHealth_OnDamageTaken(object sender, EventArgs e)
    {

        isProvoked = true;

    }

    private float GetFlatDistance(Vector3 position1, Vector3 position2)
    {
        Vector2 pos1Flat = new Vector2(position1.x, position1.z);
        Vector2 pos2Flat = new Vector2(position2.x, position2.z);
        return Vector2.Distance(pos1Flat, pos2Flat);
    }

    private bool IsPositionValidOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        Vector3 flatPosition = new Vector3(position.x, transform.position.y, position.z);
        return NavMesh.SamplePosition(flatPosition, out hit, 1.0f, NavMesh.AllAreas);
    }

    private Vector3 GetValidPositionOnNavMesh(Vector3 targetPosition)
    {
        NavMeshHit hit;
        Vector3 targetPositionFlat = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        if (NavMesh.SamplePosition(targetPositionFlat, out hit, 1.0f, NavMesh.AllAreas))
        {
            return new Vector3(hit.position.x, transform.position.y, hit.position.z);
        }
        return transform.position;
    }

    private void ReturnToWandering()
    {
        isProvoked = false;
        animator.SetBool("isMoving", true);
        animator.SetBool("isChasing", false);
        animator.SetBool("attack", false);
        navMeshAgent.speed = wanderingSpeed;
        Wander();
    }

    private void Update()
    {

        currentHp = enemyHealth.GetCurrentHealthPercentage();

        if (currentHp <= 0 && !isDead)
        {
            isDead = true;
            HandleDeath();
        }

        if (!isDead && currentHp > 0)
        {
            if (currentHp <= 50f && !howlingTriggered)
            {
                isHowling = true;
                howlingTriggered = true;
                runningSpeed = rageSpeed;
                
            }

            if (isHowling)
            {
                HandleHowling();
                return;
            }

            if (navMeshAgent.isOnNavMesh)
            {
                distanceToTarget = GetFlatDistance(targetPlayer.position, transform.position);

                if (!enemyHealth.IsDead())
                {

                    if (!IsPositionValidOnNavMesh(targetPlayer.position))
                    {
                        ReturnToWandering();
                    }
                    else if (isProvoked)
                    {
                        EngageTarget();
                    }
                    else if (distanceToTarget <= targetRange && IsPositionValidOnNavMesh(targetPlayer.position))
                    {

                        isProvoked = true;

                    }
                    else if (!isProvoked)
                    {
                        Wander();
                    }

                }
                else
                {
                    navMeshAgent.SetDestination(transform.position);

                    deathTimer += Time.deltaTime;

                    if (deathTimer > timeToDestroy)
                    {
                        Vector3 particlePosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

                    }
                }
            }
            else
            {
                isProvoked = false;
                animator.SetBool("isMoving", true);
                animator.SetBool("isChasing", false);
                animator.SetBool("attack", false);
                navMeshAgent.SetDestination(originalPos);
                navMeshAgent.speed = wanderingSpeed;

            }
        }

        if (isDead) {
            winTimer += Time.deltaTime;

            if (!checkmark.activeSelf) {
                checkmark.SetActive(true);
            }
            
            if (winTimer >= winTimerMax) {
                
                HandleWin();
            }
        }







    }

    private void EngageTarget()
    {

        Vector3 validTargetPosition = GetValidPositionOnNavMesh(targetPlayer.position);


        if (!IsPositionValidOnNavMesh(targetPlayer.position))
        {
            ReturnToWandering();
            return;
        }

        FaceTarget();

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (!currentState.IsTag("attack1") || !currentState.IsTag("attack2edited"))
        {
            if (distanceToTarget > navMeshAgent.stoppingDistance)
            {
                ChaseTarget();
            }
        }




        if (distanceToTarget <= navMeshAgent.stoppingDistance + 0.5f)
        {
            navMeshAgent.speed = 0f;
            AttackTarget();


        }
    }

    private void ChaseTarget()
    {


        if (!IsPositionValidOnNavMesh(targetPlayer.position))
        {
            ReturnToWandering();
            return;
        }


        animator.SetBool("isMoving", false);
        animator.SetBool("isChasing", true);
        animator.SetBool("attack", false);

        Vector3 validPosition = GetValidPositionOnNavMesh(targetPlayer.position);
        navMeshAgent.SetDestination(validPosition);
        navMeshAgent.speed = runningSpeed;
    }

    private void AttackTarget()
    {

        animator.SetBool("attack", true);
        animator.SetBool("isChasing", false);


    }

    private void HandleDeath()
    {   
        animator.SetTrigger("isDead");
        navMeshAgent.SetDestination(transform.position);  
        navMeshAgent.isStopped = true;  
        bossCollider.enabled = false;
    }



    private void FaceTarget()
    {



        Vector3 directionFlat = new Vector3(targetPlayer.position.x - transform.position.x, 0, targetPlayer.position.z - transform.position.z).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(directionFlat);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);


    }

    private void Wander()
    {
        timer += Time.deltaTime;

        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isMoving", true);  // Set moving animation
        }
        else
        {
            animator.SetBool("isMoving", false); // Set idle animation
        }


        if (timer >= wanderTimer)
        {

            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);


            if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
            {

                navMeshAgent.SetDestination(newPos);

            }
            else
            {
                Debug.LogWarning("NavMeshAgent is not active or has not been initialized properly.");
            }

            wanderTimer = UnityEngine.Random.Range(1f, 5f);
            timer = 0;
        }


    }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomPosition;
        NavMeshHit navHit;


        for (int i = 0; i < 30; i++)
        {
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
            randDirection += origin;


            if (NavMesh.SamplePosition(randDirection, out navHit, dist, layermask))
            {
                randomPosition = navHit.position;

                return randomPosition;
            }
        }



        return origin;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRange);
    }

    public void AttackPatternOne()
    {

        if (playerHealth == null) return;

        distanceToTarget = Vector3.Distance(playerHealth.transform.position, transform.position);

        if (distanceToTarget < 10.5f)
        {
            playerHealth.TakeDamage(30f);
            CinemachineShake.Instance.ShakeCamera(8f, 0.1f);
        }

    }

    public void AttackPatternTwo()
    {

        if (playerHealth == null) return;

        distanceToTarget = Vector3.Distance(playerHealth.transform.position, transform.position);

        if (distanceToTarget < 10.5f)
        {
            playerHealth.TakeDamage(10f);
            CinemachineShake.Instance.ShakeCamera(6f, 0.1f);
        }

    }

    public void ResetHowling()
    {
        isHowling = false;
        animator.SetBool("isHowling", false);
    }

    private void HandleHowling()
    {
        
        animator.SetBool("isHowling", true);
        

        
        
        
        animator.SetBool("isMoving", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("attack", false);

        navMeshAgent.SetDestination(transform.position);  
        navMeshAgent.speed = 0f;
    }

    private void HandleWin() {
        winGameCanvas.enabled = true;
        OnDeath?.Invoke(this, EventArgs.Empty);
        //Time.timeScale = 0;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        FindObjectOfType<FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShakePlayerCamera() {
        CinemachineShake.Instance.ShakeCameraEarthquake(7f, 3f, 4f);
        if (!earthquakeSound) {
            playerSound.EarthquakeSound();
            earthquakeSound = true;
        }
    }






}
