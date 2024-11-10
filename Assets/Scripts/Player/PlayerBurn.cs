using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBurn : MonoBehaviour
{
    public bool isBurning = false;
    private bool canBurn = true;

    public ParticleSystem playerFire;

    public float burnCooldown;
    public float burnDamagePerSecond;
    private float currentHealth;
    
    public TextMeshProUGUI healthText;

    public AudioClip playerBurnSound;

    private Coroutine burnDamageCoroutine;

    void Start() {
        currentHealth = float.Parse(healthText.text);
    }

    void OnParticleCollision(GameObject other) {
        if (other.CompareTag("EnemyFire") && canBurn && !GetComponent<HealthStatus>().dead) {
            Burn();
        }
    }

    public void Burn(){
        isBurning = true;
        canBurn = false;

        GetComponent<AudioSource>().PlayOneShot(playerBurnSound);

        currentHealth = float.Parse(healthText.text);

        playerFire.Play();

        burnDamageCoroutine = StartCoroutine(BurnDamage());
        StartCoroutine(Cooldown());
    }

    public void StopBurnDamage() {
        StopCoroutine(burnDamageCoroutine);
        isBurning = false;
        canBurn = true;
    }

    public void ApplyDamage(float damageAmount) {
        currentHealth -= damageAmount;
        healthText.text = Mathf.Max(currentHealth, 0).ToString("0");
    }

    IEnumerator BurnDamage() {
        while (isBurning && currentHealth > 0) {
            currentHealth -= burnDamagePerSecond * Time.deltaTime;

            healthText.text = Mathf.Max(currentHealth, 0).ToString("0");

            yield return null;
        }
    }

    IEnumerator Cooldown() {
        yield return new WaitForSeconds(burnCooldown);

        isBurning = false;
        canBurn = true;

        playerFire.Stop();

        currentHealth = float.Parse(healthText.text);
    }
}