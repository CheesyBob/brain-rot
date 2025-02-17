using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerFlames : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Enemy")){
            other.GetComponent<EnemyBurning>().enabled = true;
        }

        if(other.CompareTag("Casual")){
            other.GetComponent<EnemyBurning>().enabled = true;
        }
    }
}