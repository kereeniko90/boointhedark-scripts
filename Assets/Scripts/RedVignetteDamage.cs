using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RedVignetteDamage : MonoBehaviour
{
    public static RedVignetteDamage Instance { get; private set; }

    private Volume volume;

    private void Awake() {
        Instance = this;
        volume = GetComponent<Volume>();
    }

    

    private void Update() {
        if (volume.weight > 0) {
            float decreaseSpeed = 0.8f;
            volume.weight -= Time.deltaTime * decreaseSpeed;
        }
    }

    public void SetWeight(float weight) {
        volume.weight = weight;
    }
}
