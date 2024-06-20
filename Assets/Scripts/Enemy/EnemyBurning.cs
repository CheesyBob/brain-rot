using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBurning : MonoBehaviour
{
    public GameObject BurningParticles;

    public AudioClip igniteSound;
    public AudioClip[] burningVoiceLines;

    public float currentValue;
    public float decreaseRate;
    private float wanderRadius = 1000f;

    public bool hasBurned;
    public bool casual;

    private NavMeshAgent agent;

    void Start()
    {
        if(!GetComponent<EnemyDeath>().dead){
            agent = GetComponent<NavMeshAgent>();

            BurningParticles.GetComponent<ParticleSystem>().Play();

            GetComponent<AudioSource>().PlayOneShot(igniteSound);

            if(!casual){
                GetComponent<EnemyAI>().enabled = false;
                GetComponent<Animator>().SetBool("isBurning", true);
            }

            if(casual){
                GetComponent<CasualAI>().enabled = false;
            }

            StartCoroutine(PlayRandomVoiceLine());
            
            MoveToRandomPosition();
        }
    }

    void Update()
    {
        if(!GetComponent<EnemyDeath>().dead){
            currentValue -= decreaseRate * Time.deltaTime;

            if (currentValue < 0)
            {
                currentValue = 0;

                hasBurned = true;

                GetComponent<EnemyDeath>().dead = true;

                BurningParticles.GetComponent<ParticleSystem>().Stop();

                enabled = false;
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                MoveToRandomPosition();
            }
        }
    }

    IEnumerator PlayRandomVoiceLine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            if (burningVoiceLines.Length > 0 && !hasBurned)
            {
                int randomIndex = Random.Range(0, burningVoiceLines.Length);
                GetComponent<AudioSource>().PlayOneShot(burningVoiceLines[randomIndex]);
            }
        }
    }

    void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
        agent.SetDestination(navHit.position);
    }
}