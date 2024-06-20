using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SentryBullet : MonoBehaviour
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

        StartCoroutine("WaitToDestroy");
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
            Destroy(this.gameObject);
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

    IEnumerator WaitToDestroy(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}