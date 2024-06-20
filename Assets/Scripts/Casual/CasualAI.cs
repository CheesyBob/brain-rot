using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasualAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;

    private Transform player;

    private Vector3 roamDestination;

    private bool isRoaming = true;

    public float currentHealth;
    public float playerBulletDamage;
    public float audioDelay;
    public float roamRadius;
    public float runDistance;
    public float roamInterval;
    private float nextRoamTime = 0f;
    private float audioTimer;

    public AudioClip[] spottedAudioClips;
    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.Find("PlayerModel").transform;
        
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        audioTimer = audioDelay;

        SetRandomRoamDestination();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= runDistance)
            {
                RunFromPlayer();
                if (audioTimer <= 0)
                {
                    PlayRandomAudio(spottedAudioClips);
                    audioTimer = audioDelay;
                }
                else
                {
                    audioTimer -= Time.deltaTime;
                }
            }
            else
            {
                Roam();

                if (audioTimer <= 0)
                {
                    audioTimer = audioDelay;
                }
                else
                {
                    audioTimer -= Time.deltaTime;
                }
            }
        }

        if(currentHealth <= 0f){
            currentHealth = 0f;
        }

        if(currentHealth == 0f){
            GetComponent<EnemyDeath>().dead = true;
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "PlayerBullet"){
            if(currentHealth >= 0f){
                currentHealth -= playerBulletDamage;
                currentHealth = Mathf.Max(currentHealth, 0);
            }
            if(currentHealth == 0f){
                GetComponent<EnemyDeath>().dead = true;
            }
        }
    }

    void Roam()
    {
        if (isRoaming && Time.time >= nextRoamTime)
        {
            SetRandomRoamDestination();
            nextRoamTime = Time.time + roamInterval;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isRoaming = true;
        }
        else
        {
            isRoaming = false;
        }
    }

    void SetRandomRoamDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
        roamDestination = hit.position;
        agent.SetDestination(roamDestination);
    }

    void RunFromPlayer()
    {
        Vector3 runDirection = transform.position - player.position;
        Vector3 targetPosition = transform.position + runDirection.normalized * runDistance;
        agent.SetDestination(targetPosition);
    }

    void PlayRandomAudio(AudioClip[] clips)
    {
        if (clips.Length > 0 && audioSource != null && !audioSource.isPlaying)
        {
            int randomIndex = Random.Range(0, clips.Length);
            audioSource.clip = clips[randomIndex];
            audioSource.Play();
        }
    }
}