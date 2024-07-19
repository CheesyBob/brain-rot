using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public GameObject EnemyRagdoll;
    public GameObject ExecutionSkull;

    public float ragdollForce;
    public float executionChance;

    public bool dead = false;
    private bool deathAudioPlay = false;
    private bool hasCheckedRevive = false;
    public bool casual;

    public AudioClip[] deathAudioClips;
    private AudioSource audioSource;

    void Awake(){
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        EnemyRagdoll.SetActive(false);
    }

    void Update(){
        if(dead && !deathAudioPlay){
            if(!hasCheckedRevive){
                RandomChanceRevive();
                hasCheckedRevive = true;
            }

            if(dead){
                deathAudioPlay = true;
                Death();
            }
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

        ExecutionSkull.GetComponent<Canvas>().enabled = false;
    }

    void DeathAudioClips()
    {
        if (deathAudioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, deathAudioClips.Length);

            audioSource.PlayOneShot(deathAudioClips[randomIndex]);
        }
    }

    void RandomChanceRevive()
    {
        float chance = Random.Range(0f, 1f);
        if (chance <= executionChance)
        {
            dead = false;
            deathAudioPlay = false;

            EnemyRagdoll.SetActive(true);
            EnemyRagdoll.transform.SetParent(null);
            EnemyRagdoll.GetComponent<EnemyExecution>().enabled = true;

            gameObject.transform.localScale = new Vector3(0, 0, 0);

            navMeshAgent.isStopped = true;

            GetComponent<EnemyAI>().canShoot = false;

            ExecutionSkull.GetComponent<Canvas>().enabled = true;
        }
    }
}