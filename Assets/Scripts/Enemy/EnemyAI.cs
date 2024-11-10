using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public GameObject Pistol;
    public GameObject Shotgun;
    public GameObject RocketLauncher;

    private GameObject player;

    private Vector3 roamPosition;

    public float currentHealth;
    public float chaseRange;
    public float roamRadius;
    public float roamInterval;
    private float nextRoamTime;

    private bool isChasingPlayer = false;

    public bool canShoot;
    public bool EnemyPistol;
    public bool EnemyShotgun;
    public bool EnemyRocketLauncher;

    private AudioSource audioSource;
    private float audioTimer;
    private float audioDelay = 5f;
    public AudioClip[] spottedPlayerAudioClips;

    private LayerMask wallLayer;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("PlayerModel");
        audioTimer = audioDelay;

        wallLayer = LayerMask.GetMask("Wall");

        SetRandomRoamPosition();
    }

    void Update()
    {
        if (currentHealth <= 0f)
        {
            HandleDeath();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= chaseRange && CanSeePlayer())
        {
            isChasingPlayer = true;
            EnableShooting();

            if (audioTimer <= 0)
            {
                PlayRandomAudioClip();
                audioTimer = audioDelay;
            }
            else
            {
                audioTimer -= Time.deltaTime;
            }

            ChasePlayer();
        }
        else
        {
            isChasingPlayer = false;
            DisableShooting();
            Roam();
        }

        if (player.GetComponent<HealthStatus>().dead)
        {
            canShoot = false;
        }

        UpdateAnimationState();
    }

    void HandleDeath()
    {
        GetComponent<EnemyDeath>().dead = true;
    }

    void EnableShooting()
    {
        if (EnemyPistol)
            Pistol.GetComponent<PistolEnemy>().stopFire = false;
        if (EnemyShotgun)
            Shotgun.GetComponent<ShotgunEnemy>().stopFire = false;
        if (EnemyRocketLauncher)
            RocketLauncher.GetComponent<EnemyRocketLauncherShoot>().stopFire = false;
    }

    void DisableShooting()
    {
        if (EnemyPistol)
            Pistol.GetComponent<PistolEnemy>().stopFire = true;
        if (EnemyShotgun)
            Shotgun.GetComponent<ShotgunEnemy>().stopFire = true;
        if (EnemyRocketLauncher)
            RocketLauncher.GetComponent<EnemyRocketLauncherShoot>().stopFire = true;
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.5f, directionToPlayer, out hit, distanceToPlayer, wallLayer))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                return false;
            }
        }
        return true;
    }


    void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer) * Quaternion.Euler(0, -90, 0);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 360 * Time.deltaTime);
        }

        if (canShoot && CanSeePlayer())
        {
            ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {
        if (EnemyPistol && canShoot)
            Pistol.GetComponent<PistolEnemy>().FirePistol();
        if (EnemyShotgun && canShoot)
            Shotgun.GetComponent<ShotgunEnemy>().FireShotgun();
        if (EnemyRocketLauncher && canShoot)
            StartCoroutine(FireRocketDelay());
    }

    void Roam()
    {
        if (Time.time >= nextRoamTime)
        {
            SetRandomRoamPosition();
            nextRoamTime = Time.time + roamInterval;
        }

        navMeshAgent.SetDestination(roamPosition);
    }

    void SetRandomRoamPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius + transform.position;
        UnityEngine.AI.NavMeshHit hit;

        if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1))
        {
            roamPosition = hit.position;
        }
    }

    IEnumerator FireRocketDelay()
    {
        yield return new WaitForSeconds(0.4f);
        RocketLauncher.GetComponent<EnemyRocketLauncherShoot>().FireRocket();
    }

    void PlayRandomAudioClip()
    {
        if (spottedPlayerAudioClips.Length > 0 && audioSource != null && !audioSource.isPlaying && !GetComponent<EnemyDeath>().dead)
        {
            int randomIndex = Random.Range(0, spottedPlayerAudioClips.Length);
            audioSource.PlayOneShot(spottedPlayerAudioClips[randomIndex]);
        }
    }

    void UpdateAnimationState()
    {
        GetComponent<Animator>().SetBool("isRunning", navMeshAgent.velocity.magnitude > 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet") && currentHealth > 0f)
        {
            currentHealth -= other.GetComponent<PistolBullet>().damageAmount;
            currentHealth = Mathf.Max(currentHealth, 0);
        }
    }
}