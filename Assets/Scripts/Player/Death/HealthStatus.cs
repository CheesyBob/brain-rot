using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthStatus : MonoBehaviour
{
    private GameObject deathPannel;
    private GameObject weaponHolder;
    private GameObject respawnPannel;
    private GameObject painPannel;
    private TextMeshProUGUI healthText;
    public Animator playerAnimator;
    public AudioClip playerDeathSound;
    public AudioClip[] playerDamageSounds;
    public bool dead;

    private bool hasPlayedDeathSound = false;
    private int currentHealth;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneChange;
    }

    void Start()
    {
        respawnPannel = GameObject.Find("RespawnPannel");
        respawnPannel.SetActive(false);

        deathPannel = GameObject.Find("DeathPannel");
        weaponHolder = GameObject.Find("WeaponHolder");
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        painPannel = GameObject.Find("PainPannel");

        if (int.TryParse(healthText.text, out int initialHealth))
        {
            currentHealth = initialHealth;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneChange;
    }

    void Update()
    {
        if (int.TryParse(healthText.text, out int healthValue) && healthValue != currentHealth)
        {
            currentHealth = healthValue;
        }

        if (currentHealth <= 0)
        {
            dead = true;
            Death();
        }
    }

    public void Death()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<SwitchItems>().enabled = false;

        respawnPannel.SetActive(true);
        weaponHolder.SetActive(false);

        if (!hasPlayedDeathSound)
        {
            hasPlayedDeathSound = true;
            GetComponent<AudioSource>().PlayOneShot(playerDeathSound);
        }

        foreach (AnimatorControllerParameter param in playerAnimator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                if (param.name == "playerDied")
                {
                    playerAnimator.SetBool("playerDied", true);
                }
                else
                {
                    playerAnimator.SetBool(param.name, false);
                }
            }
        }

        deathPannel.GetComponent<DeathFadeIn>().isFading = true;
    }

    public void DamagePlayer(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);

        healthText.text = currentHealth.ToString();

        painPannel.GetComponent<PainFadeIn>().isFading = true;

        PlayRandomHitSounds(playerDamageSounds);

        PlayerBurn playerBurn = GetComponent<PlayerBurn>();

        if(playerBurn.isBurning)
        {
            playerBurn.ApplyDamage(damageAmount);
        }
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        hasPlayedDeathSound = false;
    }

    private void PlayRandomHitSounds(AudioClip[] hitSounds)
    {
        if (hitSounds.Length > 0)
        {
            AudioClip randomHitSound = hitSounds[Random.Range(0, hitSounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(randomHitSound);
        }
    }
}