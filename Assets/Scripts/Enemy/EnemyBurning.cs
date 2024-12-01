using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBurning : MonoBehaviour
{
    public GameObject BurningParticles;
    public AudioClip igniteSound;
    public AudioClip[] burningVoiceLines;

    public float fadeDuration;
    public float healthDecreaseRate;
    private float fadeAmount = 0.0f;

    public bool casual;

    private int materialIndex = 0;
    public SkinnedMeshRenderer[] additionalSkinnedRenderers;

    private NavMeshAgent agent;
    private bool isFading = true;

    private SkinnedMeshRenderer mainSkinnedRenderer;
    private Color originalColor;

    void Start()
    {
        if (!GetComponent<EnemyDeath>().dead)
        {
            agent = GetComponent<NavMeshAgent>();

            mainSkinnedRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            if (materialIndex < mainSkinnedRenderer.materials.Length)
            {
                originalColor = mainSkinnedRenderer.materials[materialIndex].color;
            }

            BurningParticles.GetComponent<ParticleSystem>().Play();

            GetComponent<AudioSource>().PlayOneShot(igniteSound);

            if (!casual)
            {
                GetComponent<EnemyAI>().chaseRange = 0;
                GetComponent<Animator>().SetBool("isBurning", true);
            }

            if (casual)
            {
                GetComponent<CasualAI>().detectionRadius = 0;
                GetComponent<Animator>().SetBool("isBurning", true);
            }

            StartCoroutine(PlayRandomVoiceLine());
            MoveToRandomPosition();
        }
    }

    void Update()
    {
        ApplyFadeToRenderers();

        if (!GetComponent<EnemyDeath>().dead)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                MoveToRandomPosition();
            }
        }

        if (!casual && GetComponent<EnemyAI>().currentHealth > 0)
        {
            GetComponent<EnemyAI>().currentHealth -= healthDecreaseRate * Time.deltaTime;
        }

        if (casual && GetComponent<CasualAI>().currentHealth > 0)
        {
            GetComponent<CasualAI>().currentHealth -= healthDecreaseRate * Time.deltaTime;
        }

        if (GetComponent<EnemyDeath>().dead)
        {
            BurningParticles.GetComponent<ParticleSystem>().Stop();
        }
    }

    private void ApplyFadeToRenderers()
    {
        if (isFading && fadeAmount < 1.0f)
        {
            fadeAmount += Time.deltaTime / fadeDuration;
            Color targetColor = Color.Lerp(originalColor, Color.black, fadeAmount);

            ApplyFadeToRenderer(mainSkinnedRenderer, targetColor);

            foreach (var renderer in additionalSkinnedRenderers)
            {
                ApplyFadeToRenderer(renderer, targetColor);
            }

            if (fadeAmount >= 1.0f)
            {
                isFading = false;
                fadeAmount = 1.0f;
            }
        }
    }

    private void ApplyFadeToRenderer(SkinnedMeshRenderer renderer, Color targetColor)
    {
        if (materialIndex < renderer.materials.Length)
        {
            Material[] materials = renderer.materials;
            materials[materialIndex].color = targetColor;
            renderer.materials = materials;
        }
    }

    IEnumerator PlayRandomVoiceLine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            if (burningVoiceLines.Length > 0 && !GetComponent<EnemyDeath>().dead)
            {
                int randomIndex = Random.Range(0, burningVoiceLines.Length);
                GetComponent<AudioSource>().PlayOneShot(burningVoiceLines[randomIndex]);
            }
        }
    }

    void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 1000000f;
        randomDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, 1000000f, -1);
        agent.SetDestination(navHit.position);
    }
}