using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    public AudioClip[] enemyHitSounds;

    private AudioSource enemyAudioSource;

    public float damageAmount;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy"){
            enemyAudioSource = other.gameObject.GetComponent<AudioSource>();

            PlayHitSounds(enemyHitSounds);

            if(!other.gameObject.GetComponent<EnemyDeath>().dead){
                Destroy(gameObject);
            }
        }
        if(other.gameObject.tag == "Casual"){
            enemyAudioSource = other.gameObject.GetComponent<AudioSource>();

            PlayHitSounds(enemyHitSounds);
            
            if(!other.gameObject.GetComponent<EnemyDeath>().dead){
                Destroy(gameObject);
            }
        }
        
        if(other.gameObject.tag == "Sentry"){
            other.gameObject.GetComponent<SentryShoot>().BulletDamageSentry();
        }

        if(other.gameObject.tag == "Wall"){
            Destroy(gameObject);
        }
    }

    private void PlayHitSounds(AudioClip[] hitSounds)
    {
        if (hitSounds.Length > 0)
        {
            AudioClip randomHitSound = hitSounds[Random.Range(0, hitSounds.Length)];
            enemyAudioSource.PlayOneShot(randomHitSound);
        }
    }
}