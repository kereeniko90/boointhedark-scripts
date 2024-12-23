using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectives : MonoBehaviour
{
    [SerializeField] private GameObject checkmarkOne;
    


    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "Player") {
            
            checkmarkOne.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
