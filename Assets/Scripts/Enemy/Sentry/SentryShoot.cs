using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SentryShoot : MonoBehaviour
{
    private List<GameObject> childObjects = new List<GameObject>();

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    private TextMeshProUGUI healthText;

    private Transform player;
    public Transform sentryHead;

    public ParticleSystem explosion;

    public AudioClip[] sentryHit;
    public AudioClip[] sentryShoot;
    public AudioClip sentryExplode;

    public float sentryHealth;
    public float detectRadius;
    public float rotationSpeed;
    public float playerBulletDamage;
    public float cooldownDuration;
    private float cooldownTimer = 0f;
    public float bulletSpeed;
    
    public bool dead = false;

    private LayerMask wallLayer;

    public Transform[] sentryChildTransforms;

    void Start(){
        player = GameObject.Find("PlayerModel").transform;
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        
        wallLayer = LayerMask.GetMask("Wall");
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(distanceToPlayer <= detectRadius && !dead)
        {
            if(CanSeePlayer())
            {
                Vector3 direction = player.position - sentryHead.position;

                direction = Quaternion.Euler(0, 180, 0) * direction;

                Quaternion rotation = Quaternion.LookRotation(direction);

                sentryHead.rotation = Quaternion.Lerp(sentryHead.rotation, rotation, rotationSpeed * Time.deltaTime);

                if(cooldownTimer <= 0f)
                {
                    SpawnBullet();
                    cooldownTimer = cooldownDuration;
                }
            }
        }

        cooldownTimer -= Time.deltaTime;

        if(sentryHealth <= 0 && !dead)
        {
            sentryHealth = 0;
            Explode();
        }

        if(healthText.text == "0")
        {
            dead = true;
        }
    }


    bool CanSeePlayer(){
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToPlayer, out hit, distanceToPlayer, wallLayer)){
            if (hit.collider.CompareTag("Wall")){
                return false;
            }
        }

        return true;
    }

    void SpawnBullet(){
        GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody>().linearVelocity = (player.position - bulletSpawnPoint.position).normalized * bulletSpeed;

        PlayRandomShootSound(sentryShoot);
    }

    public void Explode(){
        if(!dead){
            dead = true;

            explosion.Play();

            GetComponent<AudioSource>().PlayOneShot(sentryExplode);
            GetComponent<BoxCollider>().enabled = false;

            ApplyForceToChildren();
        }
    }

    private void ApplyForceToChildren(){
        foreach (Transform child in sentryChildTransforms){
            Rigidbody rb = child.GetComponent<Rigidbody>();

            rb.isKinematic = false;

            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDirection * 5f, ForceMode.Impulse);

            float randomRotationX = Random.Range(0f, 40f);
            float randomRotationY = Random.Range(0f, 40f);
            float randomRotationZ = Random.Range(0f, 40f);

            child.Rotate(randomRotationX, randomRotationY, randomRotationZ);
        }
    }

    public void BulletDamageSentry(){
        sentryHealth -= playerBulletDamage;
        PlayRandomHitSound(sentryHit);
    }

    void PlayRandomHitSound(AudioClip[] sentryHitSounds){
        if (sentryHitSounds.Length > 0 && !dead){
            AudioClip randomSentryHitSound = sentryHitSounds[Random.Range(0, sentryHitSounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(randomSentryHitSound);
        }
    }

    void PlayRandomShootSound(AudioClip[] sentryShootSounds){
        if (sentryShootSounds.Length > 0 && !dead){
            AudioClip randomSentryShootSound = sentryShootSounds[Random.Range(0, sentryShootSounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(randomSentryShootSound);
        }
    }
}