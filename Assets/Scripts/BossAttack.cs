using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour {

    private PlayerHealth target;

    [SerializeField] private float damage = 40f;

    

    private float distanceToTarget;

    private void Start() {
        target = FindObjectOfType<PlayerHealth>();
        
    }


    public void AttackHitEvent() {

        if (target == null) return;
        //Debug.Log("Bang");
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);

        if (distanceToTarget < 3f) {
            target.TakeDamage(damage);
        }
        


        

    }
    
}
