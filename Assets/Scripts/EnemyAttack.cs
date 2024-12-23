using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    private PlayerHealth target;

    [SerializeField] private float damage = 40f;

    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip ghostSound;

    [SerializeField] private AudioClip deathSound;
    

    
    private EnemyHealth enemyHealth;


    private float distanceToTarget;

    private AudioSource audioSource;

    private void Start() {
        target = FindObjectOfType<PlayerHealth>();
        
        audioSource = GetComponent<AudioSource>();

        enemyHealth = GetComponentInParent<EnemyHealth>();

        

        

        
    }

    public void PlayHitSound() 
    {
        
        Debug.Log("Sound Played");
    }

    public void AttackHitEvent() {

        if (target == null) return;
        //Debug.Log("Bang");
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        int randomNumber = Random.Range(0, attackSounds.Length);

        audioSource.PlayOneShot(attackSounds[randomNumber], audioSource.volume);

        if (distanceToTarget < 3.5f) {
            target.TakeDamage(damage);
            CinemachineShake.Instance.ShakeCamera(5f, 0.1f);
        }
    }

    

    public void RunEvent() {
        audioSource.PlayOneShot(ghostSound, audioSource.volume);
    }
    
}
