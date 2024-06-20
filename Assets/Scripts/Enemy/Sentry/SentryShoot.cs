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

    void Start(){
        player = GameObject.Find("PlayerModel").transform;
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
    }

    void Update(){
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(distanceToPlayer <= detectRadius && !dead){
            if(IsPlayerVisible()){
                Vector3 direction = player.position - sentryHead.position;
                Quaternion rotation = Quaternion.LookRotation(direction);

                sentryHead.rotation = Quaternion.Lerp(sentryHead.rotation, rotation, rotationSpeed * Time.deltaTime);

                if(cooldownTimer <= 0f){
                    SpawnBullet();

                    cooldownTimer = cooldownDuration;
                }
            }
        }

        cooldownTimer -= Time.deltaTime;

        if(sentryHealth <= 0 && !dead){
            sentryHealth = 0;

            Explode();
        }

        if(healthText.text == "0"){
            dead = true;
        }
    }

    bool IsPlayerVisible(){
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - sentryHead.position;

        if (Physics.Raycast(sentryHead.position, directionToPlayer, out hit, detectRadius)){
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall")){
                return false;
            }
            if (hit.collider.gameObject == player.gameObject){
                return true;
            }
        }
        return false;
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

            sentryHead.GetComponent<Rigidbody>().isKinematic = false;
            sentryHead.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
            sentryHead.gameObject.layer = LayerMask.NameToLayer("SentryGib");

            gameObject.layer = LayerMask.NameToLayer("SentryGib");

            sentryHead.Rotate(Vector3.forward * 20f);

            FindAllChildObjects(transform);
        }
    }

    private void FindAllChildObjects(Transform parent){
        foreach (Transform child in parent){
            childObjects.Add(child.gameObject);
            child.gameObject.layer = LayerMask.NameToLayer("SentryGib");
            FindAllChildObjects(child);
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