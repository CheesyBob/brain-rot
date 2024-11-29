using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public GameObject Pistol;
    public GameObject RocketLauncher;

    private GameObject player;
    private GameObject casualTarget;

    private Vector3 roamPosition;

    public float currentHealth;
    public float chaseRange;
    public float roamRadius;
    public float roamInterval;
    private float nextRoamTime;

    private bool isChasingTarget = false;

    public bool canShoot;
    public bool EnemyPistol;
    public bool EnemyRocketLauncher;
    public float bulletSpread;
    public float bulletMultiplier;

    private AudioSource audioSource;
    private float audioTimer;
    private float audioDelay = 5f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    public AudioClip[] spottedPlayerAudioClips;

    private LayerMask wallLayer;

    private float rotationYOffset = -90;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("PlayerModel");
        casualTarget = GameObject.FindGameObjectWithTag("Casual");
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

        if(currentHealth == 0f){
            HandleDeath();
            return;
        }

        if (casualTarget == null)
        {
            casualTarget = player;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceToCasual = Vector3.Distance(transform.position, casualTarget.transform.position);

        bool canSeePlayer = CanSeeTarget(player, distanceToPlayer);
        bool canSeeCasual = CanSeeTarget(casualTarget, distanceToCasual);

        if ((canSeePlayer && distanceToPlayer <= chaseRange) || (canSeeCasual && distanceToCasual <= chaseRange))
        {
            isChasingTarget = true;
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

            if (canSeePlayer)
                ChaseTarget(player);
            else
                ChaseTarget(casualTarget);
        }
        else
        {
            isChasingTarget = false;
            DisableShooting();
            Roam();
        }

        UpdateAnimationState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet") && currentHealth > 0f)
        {
            currentHealth -= other.GetComponent<PistolBullet>().damageAmount;
            currentHealth = Mathf.Max(currentHealth, 0);
        }
    }

    void HandleDeath()
    {
        GetComponent<EnemyDeath>().dead = true;
    }

    void EnableShooting()
    {
        if (EnemyPistol)
            Pistol.GetComponent<PistolEnemy>().stopFire = false;
        if (EnemyRocketLauncher)
            RocketLauncher.GetComponent<EnemyRocketLauncherShoot>().stopFire = false;
    }

    public void DisableShooting()
    {
        if (EnemyPistol)
            Pistol.GetComponent<PistolEnemy>().stopFire = true;
        if (EnemyRocketLauncher)
            RocketLauncher.GetComponent<EnemyRocketLauncherShoot>().stopFire = true;
    }

    bool CanSeeTarget(GameObject target, float distanceToTarget)
    {
        Vector3 directionToTarget = target.transform.position - transform.position;

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.5f, directionToTarget, out hit, distanceToTarget, wallLayer))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                return false;
            }
        }
        return true;
    }

    void ChaseTarget(GameObject target)
    {
        navMeshAgent.SetDestination(target.transform.position);

        RotateTowardsEnemy(target);

        if (canShoot && CanSeeTarget(target, Vector3.Distance(transform.position, target.transform.position)))
        {
            ShootAtTarget(target);
        }
    }

    void ShootWithSpread(GameObject bulletPrefab, Transform firePoint, GameObject target)
    {
        int totalBullets = (int)bulletMultiplier;
        totalBullets = Mathf.Max(totalBullets, 1);

        for (int i = 0; i < totalBullets; i++)
        {
            Vector3 spread = Vector3.zero;

            if (totalBullets > 1)
            {
                spread = new Vector3(
                    Random.Range(-bulletSpread, bulletSpread),
                    Random.Range(-bulletSpread, bulletSpread),
                    Random.Range(-bulletSpread, bulletSpread)
                );
            }

            Vector3 direction = (target.transform.position - firePoint.position).normalized + spread;

            Quaternion bulletRotation = Quaternion.LookRotation(direction);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        }
    }


    void ShootAtTarget(GameObject target)
    {
        if (canShoot && Time.time >= nextFireTime)
        {
            if (EnemyPistol)
            {
                Transform pistolFirePoint = Pistol.GetComponent<PistolEnemy>().muzzlePoint.gameObject.transform;
                GameObject bulletPrefab = Pistol.GetComponent<PistolEnemy>().bulletPrefab;

                ShootWithSpread(bulletPrefab, pistolFirePoint, target);
                Pistol.GetComponent<PistolEnemy>().Fire();
            }

            if (EnemyRocketLauncher)
            {
                StartCoroutine(FireRocketDelay());
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    void Roam()
    {
        if (Time.time >= nextRoamTime)
        {
            SetRandomRoamPosition();
            nextRoamTime = Time.time + roamInterval;
        }

        navMeshAgent.SetDestination(roamPosition);

        RotateTowardsRoamTarget();
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

    void RotateTowardsEnemy(GameObject target)
    {
        Vector3 directionToEnemy = target.transform.position - transform.position;
        directionToEnemy.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
        targetRotation *= Quaternion.Euler(0, rotationYOffset, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void RotateTowardsRoamTarget()
    {
        Vector3 directionToTarget = roamPosition - transform.position;
        directionToTarget.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        targetRotation *= Quaternion.Euler(0, rotationYOffset, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}