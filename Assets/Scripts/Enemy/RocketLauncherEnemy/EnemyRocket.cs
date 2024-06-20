using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyRocket : MonoBehaviour
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

        StartCoroutine("WaitToDestroy");
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Wall"){
            Explode();
        }
        if(other.gameObject.tag == "PlayerModel"){
            Explode();
        }
    }

    void Explode(){
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, radius);

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

    void Update(){
        GameObject[] EnemiesRocketLauncher = GameObject.FindGameObjectsWithTag("EnemyRocketLauncher");

        if(healthText.text == "0"){
            foreach(GameObject Enemy in EnemiesRocketLauncher){
                Enemy.GetComponent<EnemyRocketLauncherShoot>().stopFire = true;
                Enemy.GetComponent<EnemyRocketLauncherShoot>().enabled = false;
            }
        }
    }

    IEnumerator WaitToDestroy(){
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }
}