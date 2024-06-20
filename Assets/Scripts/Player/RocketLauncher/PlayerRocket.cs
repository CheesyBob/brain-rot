using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRocket : MonoBehaviour
{
    private GameObject PainPannel;

    private TextMeshProUGUI healthText;

    public AudioClip explosionSound;
    public AudioClip[] playerHitSounds;

    public float radius;

    public int damageAmount;

    void Start(){
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        PainPannel = GameObject.Find("PainPannel");

        Destroy(gameObject, 6f);
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Wall"){
            Explode();
        }

        if(other.gameObject.tag == "Enemy"){
            Explode();
        }

        if(other.gameObject.tag == "Casual"){
            Explode();
        }

        if(other.gameObject.tag == "Sentry"){
            other.gameObject.GetComponent<SentryShoot>().Explode();

            Explode();
        }

        if(other.gameObject.tag == "Ground"){
            Explode();
        }
    }

    void Explode(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy", "Casual"));
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in colliders){
            float distance = Vector3.Distance(transform.position, col.transform.position);

            if (distance <= radius){
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
                if(int.TryParse(healthText.text, out int currentHealth)){
                    currentHealth -= damageAmount;
                    currentHealth = Mathf.Max(currentHealth, 0);

                    healthText.text = currentHealth.ToString();
                }
                PainPannel.GetComponent<PainFadeIn>().isFading = true;

                PlayHitSounds(playerHitSounds);
            }
        }

        GetComponent<MeshRenderer>().enabled = false;

        GetComponent<CapsuleCollider>().enabled = false;

        GetComponent<Rigidbody>().isKinematic = true;

        GetComponent<AudioSource>().PlayOneShot(explosionSound);

        Transform firstChild = transform.GetChild(0);
        firstChild.gameObject.SetActive(true);

        Transform secondChild = transform.GetChild(1);
        secondChild.gameObject.GetComponent<ParticleSystem>().Stop();

        Destroy(gameObject, 2f);
    }

    private void PlayHitSounds(AudioClip[] hitSounds)
    {
        if(hitSounds.Length > 0)
        {
            AudioClip randomHitSound = hitSounds[Random.Range(0, hitSounds.Length)];
            GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(randomHitSound);
        }
    }
}