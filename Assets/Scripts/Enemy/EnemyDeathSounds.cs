using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathSounds : MonoBehaviour
{
    public AudioClip[] deathAudioClips;
    private bool hasPlayed = false;

    void OnEnable(){
        DeathAudioClips();
    }

    void DeathAudioClips()
    {
        if (deathAudioClips.Length > 0 && !hasPlayed)
        {
            int randomIndex = Random.Range(0, deathAudioClips.Length);

            GetComponent<AudioSource>().PlayOneShot(deathAudioClips[randomIndex]);

            hasPlayed = true;
        }
    }
}