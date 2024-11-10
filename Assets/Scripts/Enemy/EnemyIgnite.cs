using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIgnite : MonoBehaviour
{
    private bool callBurnVoid = true;

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
        if(other.CompareTag("PlayerModel") && callBurnVoid){
            other.GetComponent<PlayerBurn>().Burn();

            callBurnVoid = false;

            StartCoroutine("callBurnCooldown");
        }
    }

    IEnumerator callBurnCooldown(){
        yield return new WaitForSeconds(GameObject.Find("PlayerModel").GetComponent<PlayerBurn>().burnCooldown);
        callBurnVoid = true;
    }
}