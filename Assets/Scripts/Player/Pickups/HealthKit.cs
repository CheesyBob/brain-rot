using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthKit : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    public int healthAmount;
    public int maxHealth;

    private AudioSource audioSource;
    public AudioClip HealSound;
    public AudioClip maxHealthSound;

    void Start(){
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "PlayerModel"){
            if(int.TryParse(healthText.text, out int currentHealth)){
                int newHealth = currentHealth + healthAmount;
                newHealth = Mathf.Clamp(newHealth, 0, maxHealth);

            if(currentHealth == maxHealth){
                audioSource.clip = maxHealthSound;
                audioSource.Play();
                return;
            }

            if(currentHealth <= maxHealth){
                healthText.text = newHealth.ToString();
                audioSource.clip = HealSound;
                audioSource.Play();
                Destroy(this.gameObject, 0.22f);
                }
            }
        }
    }
}