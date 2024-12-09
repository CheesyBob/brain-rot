using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public GameObject EnemyRagdoll;

    public float ragdollForce;

    public bool dead = false;
    public bool casual;
    private bool hasPlayed = false;

    void Awake(){
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        EnemyRagdoll.SetActive(false);
    }

    void Update(){
        if(dead && !hasPlayed){
            Death();
        }
    }

    public void Death(){
        EnemyRagdoll.SetActive(true);
        EnemyRagdoll.transform.SetParent(null);
        EnemyRagdoll.GetComponent<Rigidbody>().AddForce(transform.forward * ragdollForce, ForceMode.Impulse);

        gameObject.transform.localScale = new Vector3(0, 0, 0);

        navMeshAgent.isStopped = true;

        EnemyRagdoll.GetComponent<EnemyDeathSounds>().enabled = true;
        
        Destroy(gameObject);
    }
}