using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour {

    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float timer;
    private float timerMax;
    private float startingIntensity;
    private float startingFrequency;

    
    private void Awake() {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update() {
        if (timer < timerMax) {
            timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(startingIntensity, 0f , timer/timerMax);
            float frequency = Mathf.Lerp(startingFrequency, 0f, timer / timerMax);
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency; 
        }
    }

    public void ShakeCamera(float intensity, float timerMax) {
        this.timerMax = timerMax;
        timer = 0f;
        startingIntensity = intensity;
        startingFrequency = 1f;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }

    public void ShakeCameraEarthquake(float intensity, float frequency, float timerMax) {
        this.timerMax = timerMax;
        timer = 0f;
        startingIntensity = intensity;
        startingFrequency = frequency;  // 
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
    }
    
}
