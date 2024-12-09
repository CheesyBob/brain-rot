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

    public bool EnemyPistol;
    public bool EnemyRocketLauncher;
    private bool canPlayPlayerSpottedClips = true;
    public float bulletSpread;
    public float bulletMultiplier;

    private AudioSource audioSource;

    public AudioClip[] spottedPlayerAudioClips;

    private LayerMask wallLayer;

    private float rotationYOffset = -90;
    private float fireRate = 0.5f;
    private float lastFireTime = 0f;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("PlayerModel");

        wallLayer = LayerMask.GetMask("Wall");

        SetRandomRoamPosition();
        navMeshAgent.updateRotation = false;
    }

    void Update()
    {
        if (currentHealth <= 0f)
        {
            HandleDeath();
            return;
        }

        UpdateCasualTargets();

        GameObject target = GetPriorityTarget();

        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            bool canSeeTarget = CanSeeTarget(target, distanceToTarget);

            if (canSeeTarget && distanceToTarget <= chaseRange)
            {
                isChasingTarget = true;
                EnableShooting();
                PlayRandomAudioClip();

                ChaseTarget(target);
            }
            else
            {
                isChasingTarget = false;
                DisableShooting();
                Roam();
            }
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

    void DisableShooting()
    {
        if (EnemyPistol)
            Pistol.GetComponent<PistolEnemy>().stopFire = true;
        if (EnemyRocketLauncher)
            RocketLauncher.GetComponent<EnemyRocketLauncherShoot>().stopFire = true;
    }

    void UpdateCasualTargets()
    {
        GameObject[] casuals = GameObject.FindGameObjectsWithTag("Casual");

        if (casuals.Length > 0)
        {
            casualTarget = GetClosestTarget(casuals);
        }
        else
        {
            casualTarget = null;
        }
    }

    GameObject GetClosestTarget(GameObject[] targets)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < minDistance)
            {
                closest = target;
                minDistance = distance;
            }
        }

        return closest;
    }

    GameObject GetPriorityTarget()
    {
        if (casualTarget != null && Vector3.Distance(transform.position, casualTarget.transform.position) < chaseRange)
        {
            return casualTarget;
        }

        if (player != null && Vector3.Distance(transform.position, player.transform.position) < chaseRange)
        {
            return player;
        }

        return null;
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

        if (CanSeeTarget(target, Vector3.Distance(transform.position, target.transform.position)))
        {
            ShootAtTarget(target);
        }
    }

    void ShootWithSpread(GameObject bulletPrefab, Transform firePoint, GameObject target)
    {
        // Check if enough time has passed since the last shot
        if (Time.time - lastFireTime < fireRate)
            return; // If not, return without firing

        int totalBullets = (int)bulletMultiplier;
        totalBullets = Mathf.Max(totalBullets, 0);

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
            Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        }

        // Update the last fire time after shooting
        lastFireTime = Time.time;
    }

    void ShootAtTarget(GameObject target)
    {
        if (EnemyPistol)
        {
            Transform pistolFirePoint = Pistol.GetComponent<PistolEnemy>().muzzlePoint;
            GameObject bulletPrefab = Pistol.GetComponent<PistolEnemy>().bulletPrefab;

            ShootWithSpread(bulletPrefab, pistolFirePoint, target);

            Pistol.GetComponent<PistolEnemy>().Fire();
        }

        if (EnemyRocketLauncher)
        {
            StartCoroutine(FireRocketDelay());
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
        if (spottedPlayerAudioClips.Length > 0 && !audioSource.isPlaying && !GetComponent<EnemyDeath>().dead && canPlayPlayerSpottedClips)
        {
            canPlayPlayerSpottedClips = false;

            int randomIndex = Random.Range(0, spottedPlayerAudioClips.Length);
            audioSource.PlayOneShot(spottedPlayerAudioClips[randomIndex]);

            StartCoroutine("PlayerSpottedCooldown");
        }
    }

    IEnumerator PlayerSpottedCooldown()
    {
        yield return new WaitForSeconds(2);
        canPlayPlayerSpottedClips = true;
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

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 360f);
    }

    void RotateTowardsRoamTarget()
    {
        Vector3 directionToTarget = roamPosition - transform.position;
        directionToTarget.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        targetRotation *= Quaternion.Euler(0, rotationYOffset, 0);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 360f);
    }
}