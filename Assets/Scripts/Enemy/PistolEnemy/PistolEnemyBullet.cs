using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PistolEnemyBullet : MonoBehaviour
{
    private GameObject Player;
    private GameObject PainPannel;

    public AudioClip[] playerHitSounds;

    private AudioSource playerAudioSource;

    private TextMeshProUGUI healthText;

    public int damageAmount;

    void Start(){
        Player = GameObject.Find("Player");
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        PainPannel = GameObject.Find("PainPannel");

        playerAudioSource = Player.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "PlayerModel"){
            PlayHitSounds(playerHitSounds);

            if(int.TryParse(healthText.text, out int currentHealth)){
                currentHealth -= damageAmount;
                currentHealth = Mathf.Max(currentHealth, 0);

                healthText.text = currentHealth.ToString();
            }

            PainPannel.GetComponent<PainFadeIn>().isFading = true;

            Destroy(gameObject);
        }
        if(other.tag == "Wall"){
            Destroy(gameObject);
        }
    }

    private void PlayHitSounds(AudioClip[] hitSounds)
    {
        if (hitSounds.Length > 0)
        {
            AudioClip randomHitSound = hitSounds[Random.Range(0, hitSounds.Length)];
            playerAudioSource.PlayOneShot(randomHitSound);
        }
    }

    void Update(){
        GameObject[] EnemiesPistol = GameObject.FindGameObjectsWithTag("EnemyPistol");
        GameObject[] EnemiesShotgun = GameObject.FindGameObjectsWithTag("EnemyShotgun");

        if(healthText.text == "0"){
            foreach(GameObject Enemy in EnemiesPistol){
                Enemy.GetComponent<PistolEnemy>().stopFire = true;
                Enemy.GetComponent<PistolEnemy>().enabled = false;
            }
        }

        if(healthText.text == "0"){
            foreach(GameObject Enemy in EnemiesShotgun){
                Enemy.GetComponent<ShotgunEnemy>().stopFire = true;
                Enemy.GetComponent<ShotgunEnemy>().enabled = false;
            }
        }
    }
}