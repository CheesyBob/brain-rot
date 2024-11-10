using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarrelExplode : MonoBehaviour
{
    public float currentHealth;
    public ParticleSystem explosion;
    public ParticleSystem fire;

    public MeshFilter barrelMesh;
    public Mesh explodedBarrelMesh;

    public AudioClip[] explosionSounds;
    public AudioClip[] hitSounds;

    private bool hasExploded = false;

    public void Update(){
        if(currentHealth <= 0){
            currentHealth = 0;
        }

        if(currentHealth == 0){
            Explode();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerBullet"))
        {
            currentHealth -= other.GetComponent<PistolBullet>().damageAmount;
            currentHealth = Mathf.Max(currentHealth, 0);

            PlayRandomHitSound(hitSounds);

            Destroy(other.gameObject);
        }
    }

    public void Explode(){
        if(!hasExploded){
            hasExploded = true;
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("Enemy", "Casual"));
            Collider[] playerColliders = Physics.OverlapSphere(transform.position, 5);

            foreach (Collider col in colliders){
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance <= 5){
                    EnemyAI component = col.GetComponent<EnemyAI>();
                    if (component != null) {
                        component.currentHealth -= 100f;
                    }

                    if(col.CompareTag("Casual")){
                        col.GetComponent<CasualAI>().currentHealth -= 100f;
                    }
                }
            }

            foreach (Collider col in playerColliders){
                if(col.CompareTag("PlayerModel")){
                    col.GetComponent<HealthStatus>().DamagePlayer(50);
                }
            }

            explosion.gameObject.transform.SetParent(null);
            fire.gameObject.transform.SetParent(null);
            fire.gameObject.SetActive(true);

            explosion.Play();
            fire.Play();

            PlayRandomExplosionSound(explosionSounds);

            GetComponent<Rigidbody>().isKinematic = false;

            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)).normalized;

            GetComponent<Rigidbody>().AddForce(randomDirection * 10f, ForceMode.Impulse);

            float randomRotationX = Random.Range(0f, 40f);
            float randomRotationY = Random.Range(0f, 40f);
            float randomRotationZ = Random.Range(0f, 40f);

            gameObject.transform.Rotate(randomRotationX, randomRotationY, randomRotationZ);
        
            barrelMesh.mesh = explodedBarrelMesh;

            Destroy(explosion.gameObject, 2);
            Destroy(fire.gameObject, 15);
        }
    }

    public void PlayRandomHitSound(AudioClip[] barrelHitSounds){
        if (barrelHitSounds.Length > 0){
            AudioClip randomBarrelHitSound = barrelHitSounds[Random.Range(0, barrelHitSounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(randomBarrelHitSound);
        }
    }

    public void PlayRandomExplosionSound(AudioClip[] explosionSounds){
        if (explosionSounds.Length > 0){
            AudioClip randomExplosionSound = explosionSounds[Random.Range(0, explosionSounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(randomExplosionSound);
        }
    }
}