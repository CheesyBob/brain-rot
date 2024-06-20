using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthStatus : MonoBehaviour
{
    private GameObject DeathPannel;
    private GameObject WeaponHolder;

    private GameObject respawnPannel;

    private TextMeshProUGUI healthText;

    private AudioSource deathAudioSource;
    
    public AudioClip[] deathSounds;
    public AudioClip bodyImpactSound;

    public Animator playerAnimator;

    private float waitToPlayBodyImpact;

    public bool death;

    private bool hasPlayedDeathSound = false;
    private bool hasPlayedBodyImpactSound = false;

    void Awake(){
        respawnPannel = GameObject.Find("RespawnPannel");

        respawnPannel.SetActive(false);
    }

    void Start(){
        DeathPannel = GameObject.Find("DeathPannel");
        WeaponHolder = GameObject.Find("WeaponHolder");
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();

        deathAudioSource = GetComponent<AudioSource>();
    }

    void Update(){
        if(healthText.text == "0"){
            death = true;

            Death();
        }
    }

    public void Death(){
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<SwitchItems>().enabled = false;

        respawnPannel.SetActive(true);

        WeaponHolder.SetActive(false);

        if(!hasPlayedDeathSound){
            hasPlayedDeathSound = true;

            PlayRandomDeathSound(deathSounds);
        }

        if(Random.Range(0f, 1f) < 0.5f){
            playerAnimator.SetBool("playerDied", true);

            waitToPlayBodyImpact = .2f;

            StartCoroutine("PlayImpactSound");
        }
        else{
            playerAnimator.SetBool("playerDied2", true);

            waitToPlayBodyImpact = .36f;

            StartCoroutine("PlayImpactSound");
        }

        DeathPannel.GetComponent<DeathFadeIn>().isFading = true;
    }

    IEnumerator PlayImpactSound(){
        yield return new WaitForSeconds(waitToPlayBodyImpact);
        if(!hasPlayedBodyImpactSound){
            deathAudioSource.PlayOneShot(bodyImpactSound);

            hasPlayedBodyImpactSound = true;
        }
    }

    public void PlayRandomDeathSound(AudioClip[] deathSound){
        if (deathSound.Length > 0)
        {
            AudioClip randomDeathSound = deathSound[Random.Range(0, deathSound.Length)];
            
            GetComponent<AudioSource>().clip = randomDeathSound;
            GetComponent<AudioSource>().Play();
        }
    }
}