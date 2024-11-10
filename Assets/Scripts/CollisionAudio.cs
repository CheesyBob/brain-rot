using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    public AudioClip[] collisionSounds;

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ground")){
            PlayRandomCollisionSound(collisionSounds);
        }
    }

    void PlayRandomCollisionSound(AudioClip[] collisionSoundsSounds)
    {
        if (collisionSoundsSounds.Length > 0)
        {
            AudioClip randomcollisionSoundsSound = collisionSoundsSounds[Random.Range(0, collisionSoundsSounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(randomcollisionSoundsSound);
        }
    }
}