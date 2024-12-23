using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVision : MonoBehaviour {

    private Transform targetPlayer;

    private void Start() {
        targetPlayer = GameObject.Find("pfPlayer").GetComponent<Transform>();
    }

    public bool PlayerInSight() {
        RaycastHit hit;
        Vector3 directionToPlayer = targetPlayer.position - transform.position;

        
        if (Physics.Raycast(transform.position, directionToPlayer, out hit)) {
        
            if (hit.transform == targetPlayer) {
                return true;  
            }
    }

    return false;
    }
    
}
