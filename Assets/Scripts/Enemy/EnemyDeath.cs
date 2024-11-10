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

    public AudioClip[] deathAudioClips;
    private AudioSource audioSource;

    void Awake(){
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        EnemyRagdoll.SetActive(false);
    }

    void Update(){
        if(dead && !hasPlayed){
            Death();
        }
    }

    public void Death(){
        DeathAudioClips();

        EnemyRagdoll.SetActive(true);
        EnemyRagdoll.transform.SetParent(null);
        EnemyRagdoll.GetComponent<Rigidbody>().AddForce(transform.forward * ragdollForce, ForceMode.Impulse);

        gameObject.transform.localScale = new Vector3(0, 0, 0);

        navMeshAgent.isStopped = true;

        if(!casual){
            GetComponent<EnemyAI>().canShoot = false;
        }
    }

    void DeathAudioClips()
    {
        if (deathAudioClips.Length > 0 && !hasPlayed)
        {
            int randomIndex = Random.Range(0, deathAudioClips.Length);

            audioSource.PlayOneShot(deathAudioClips[randomIndex]);

            hasPlayed = true;
        }
    }
}