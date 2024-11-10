using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MolotovFire : MonoBehaviour
{
    public void OnEnable(){
        StartCoroutine("DestroyFire");
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Enemy")){
            other.GetComponent<EnemyBurning>().enabled = true;
        }

        if(other.CompareTag("Casual")){
            other.GetComponent<EnemyBurning>().enabled = true;
        }
    }

    IEnumerator DestroyFire(){
        yield return new WaitForSeconds(10f);
        GetComponent<ParticleSystem>().Stop();
        GetComponent<AudioSource>().Pause();
        GetComponent<MolotovFire>().enabled = false;
    }
}