using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireIgnite : MonoBehaviour
{
    void OnParticleCollision(GameObject other){
        if(other.CompareTag("Enemy")){
            other.GetComponent<EnemyBurning>().enabled = true;
        }

        if(other.CompareTag("Casual")){
            other.GetComponent<EnemyBurning>().enabled = true;
        }
        if(other.CompareTag("Sentry")){
            other.GetComponent<SentryShoot>().Explode();
        }
    }
}