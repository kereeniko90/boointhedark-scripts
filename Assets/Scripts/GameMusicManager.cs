using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip track1;
    [SerializeField] private AudioClip track2;
    [SerializeField] private AudioClip track3;

    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic();
    }

    public void PlayBossMusic () {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = track3;
        audioSource.Play();
    }

    public void PlayBackgroundMusic () {
        audioSource.Stop();
        audioSource.loop = false;
        int randomNumber = Random.Range (0,2);

        if (randomNumber == 0) {
            audioSource.clip = track1;
        } else {
            audioSource.clip = track2;
        }
    }

    private void Update() {
        if (!audioSource.isPlaying && audioSource.clip != track3) {
            PlayBackgroundMusic();
        }
    }
}
