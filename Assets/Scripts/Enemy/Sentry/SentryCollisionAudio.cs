using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryCollisionAudio : MonoBehaviour
{
    public AudioClip[] sentryCollision;

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ground")){
            PlayRandomCollisionSound(sentryCollision);
        }
    }

    void PlayRandomCollisionSound(AudioClip[] sentryCollisionSounds)
    {
        if (sentryCollisionSounds.Length > 0)
        {
            AudioClip randomSentryCollisionSound = sentryCollisionSounds[Random.Range(0, sentryCollisionSounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(randomSentryCollisionSound);
        }
    }
}