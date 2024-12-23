using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour {

    [SerializeField] private AudioClip bossStep1;
    [SerializeField] private AudioClip bossStep2;
    [SerializeField] private AudioClip bossStep3;
    [SerializeField] private AudioClip[] bossGrowls;
    [SerializeField] private AudioClip bossShortGrowl;

    [SerializeField] private AudioClip[] bossAttacks;
    [SerializeField] private AudioClip bossHowl;
    [SerializeField] private AudioClip bossDeath;
    

    private AudioSource audioSource;
    

    private void Start() {
        audioSource = GetComponent<AudioSource>();

    }

    public void BossStepOne() {
        audioSource.PlayOneShot(bossStep1);
    }

    public void BossStepTwo() {
        audioSource.PlayOneShot(bossStep1);
    }

    public void BossStepThree() {
        audioSource.PlayOneShot(bossStep1);
    }

    public void AttackSound() {
        int randomIndex = Random.Range(0,bossAttacks.Length);
        audioSource.PlayOneShot(bossAttacks[randomIndex]);
    }

    public void GrowlSound() {
        int randomIndex = Random.Range(0,bossGrowls.Length);
        audioSource.PlayOneShot(bossGrowls[randomIndex]);
    }

    public void ShortGrowlSound() {
        audioSource.PlayOneShot(bossShortGrowl);
    }

    public void ShortGrowlSoundRandom() {
        int random = Random.Range(0,2);

        if (random == 1) {
            audioSource.PlayOneShot(bossShortGrowl);
        }
    }

    public void PlayHowl() {
        audioSource.PlayOneShot(bossHowl);
        
    }

    public void PlayDeathSound() {
        audioSource.PlayOneShot(bossDeath);
    }
    
}
