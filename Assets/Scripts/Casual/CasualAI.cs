using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CasualAI : MonoBehaviour
{
    public float currentHealth;
    public float detectionRadius;
    public float chaseSpeed;
    private float rotationYOffset = -90;

    [Header("Casual Type")]
    public bool casualPistol;
    public bool casualRocketlauncher;

    [Header("Weapon Scripts")]
    public CasualPistol pistol;
    public CasualRocketLauncher rocketLauncher;

    private NavMeshAgent agent;
    public GameObject targetEnemy;
    private GameObject player;
    private LayerMask wallLayer;

    public AudioClip[] playerSpotClips;
    public AudioClip[] killClips;

    private bool hasPlayedPlayerClips = false;
    public float roamRadius;
    public float roamSpeed;
    private Vector3 randomRoamTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wallLayer = LayerMask.NameToLayer("Wall");
        player = GameObject.FindGameObjectWithTag("PlayerModel");
        agent.speed = roamSpeed;
        SetNewRoamTarget();
    }

    void Update()
    {
        if(currentHealth == 0f){
            GetComponent<EnemyDeath>().dead = true;
        }

        DetectPlayer();

        if (targetEnemy == null)
        {
            FindNearestEnemy();
        }
        else
        {
            EnemyDeath enemyDeathScript = targetEnemy.GetComponent<EnemyDeath>();

            if (enemyDeathScript != null && enemyDeathScript.dead)
            {
                targetEnemy = null;

                EnemyKillClips();
                FindNearestEnemy();
            }
        }

        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            FollowPlayer();
            FindNearestEnemy();

            if (targetEnemy != null)
            {
                RotateTowardsEnemy();

                if (!IsPathThroughWall(targetEnemy.transform.position))
                {
                    agent.SetDestination(targetEnemy.transform.position);
                    if (casualPistol && HasLineOfSight(targetEnemy.transform))
                    {
                        pistol.FirePistol();
                        UpdateAnimationState();
                    }
                    if (casualRocketlauncher && HasLineOfSight(targetEnemy.transform)){
                        rocketLauncher.FireRocket();
                    }
                }
                else
                {
                    agent.ResetPath();
                }
            }
        }
        else
        {
            FindNearestEnemy();

            if (targetEnemy != null)
            {
                if (!IsPathThroughWall(targetEnemy.transform.position))
                {
                    agent.SetDestination(targetEnemy.transform.position);
                    RotateTowardsEnemy();

                    if (casualPistol && HasLineOfSight(targetEnemy.transform))
                    {
                        pistol.FirePistol();
                        UpdateAnimationState();
                    }
                }
                else
                {
                    agent.ResetPath();
                }
            }
            else
            {
                RoamAround();
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "EnemyBullet"){
            if(currentHealth >= 0f){
                currentHealth -= 5f;
                currentHealth = Mathf.Max(currentHealth, 0);
            }
        }
    }

    void EnemyKillClips()
    {
        if (killClips.Length > 0)
        {
            int randomIndex = Random.Range(0, killClips.Length);
            GetComponent<AudioSource>().PlayOneShot(killClips[randomIndex]);
        }
    }

    void DetectPlayer()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRadius);
        bool playerDetected = false;

        foreach (Collider col in players)
        {
            if (col.gameObject.CompareTag("PlayerModel"))
            {
                playerDetected = true;
                if (player == null || player != col.gameObject)
                {
                    player = col.gameObject;
                    PlayerDetectedClips();
                }
            }
        }

        if (!playerDetected && player != null)
        {
            player = null;
        }
    }

    void PlayerDetectedClips()
    {
        if (playerSpotClips.Length > 0 && !hasPlayedPlayerClips)
        {
            int randomIndex = Random.Range(0, playerSpotClips.Length);
            GetComponent<AudioSource>().PlayOneShot(playerSpotClips[randomIndex]);
            hasPlayedPlayerClips = true;
        }
    }

    void FollowPlayer()
    {
        agent.SetDestination(player.transform.position);

        RotateTowardsPlayer();
        UpdateAnimationState();
    }

    void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        targetRotation *= Quaternion.Euler(0, rotationYOffset, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void RotateTowardsEnemy()
    {
        if (targetEnemy != null)
        {
            Vector3 directionToEnemy = targetEnemy.transform.position - transform.position;
            directionToEnemy.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

            targetRotation *= Quaternion.Euler(0, rotationYOffset, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        float nearestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (HasLineOfSight(collider.transform))
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                    if (distanceToEnemy < nearestDistance)
                    {
                        nearestDistance = distanceToEnemy;
                        nearestEnemy = collider.gameObject;
                    }
                }
            }
        }
        targetEnemy = nearestEnemy;
    }

    bool HasLineOfSight(Transform enemyTransform)
    {
        Vector3 directionToEnemy = enemyTransform.position - transform.position;
        float distanceToEnemy = directionToEnemy.magnitude;

        Vector3 rayOrigin = transform.position + Vector3.up * 1.0f;
        Ray ray = new Ray(rayOrigin, directionToEnemy.normalized);
        RaycastHit hit;

        Debug.DrawRay(rayOrigin, directionToEnemy.normalized * distanceToEnemy, Color.green, 0.1f);

        if (Physics.Raycast(ray, out hit, distanceToEnemy))
        {
            if (hit.collider.gameObject == enemyTransform.gameObject)
            {
                return true;
            }
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                return false;
            }
        }
        return false;
    }

    bool IsPathThroughWall(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Vector3 start = path.corners[i];
            Vector3 end = path.corners[i + 1];

            if (Physics.Linecast(start, end, wallLayer))
            {
                return true;
            }
        }
        return false;
    }

    void RotateTowardsTarget()
    {
        Vector3 directionToTarget = randomRoamTarget - transform.position;
        directionToTarget.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        targetRotation *= Quaternion.Euler(0, rotationYOffset, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void UpdateAnimationState()
    {
        GetComponent<Animator>().SetBool("isRunning", GetComponent<NavMeshAgent>().velocity.magnitude > 0);
    }

    void RoamAround()
    {
        if (Vector3.Distance(transform.position, randomRoamTarget) < 1f && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetNewRoamTarget();
        }

        agent.SetDestination(randomRoamTarget);

        agent.speed = roamSpeed;

        RotateTowardsRoamTarget();
        UpdateAnimationState();
    }

    void RotateTowardsRoamTarget()
    {
        Vector3 directionToTarget = randomRoamTarget - transform.position;
        directionToTarget.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        targetRotation *= Quaternion.Euler(0, rotationYOffset, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void SetNewRoamTarget()
    {
        Vector3 newTarget = Vector3.zero;
        bool validDestination = false;

        while (!validDestination)
        {
            newTarget = new Vector3(
                transform.position.x + Random.Range(-roamRadius, roamRadius),
                transform.position.y,
                transform.position.z + Random.Range(-roamRadius, roamRadius)
            );

            if (Vector3.Distance(newTarget, transform.position) >= 5f && IsDestinationValid(newTarget))
            {
                validDestination = true;
            }
        }

        randomRoamTarget = newTarget;
    }

    bool IsDestinationValid(Vector3 destination)
    {
        NavMeshHit navMeshHit;

        if (!NavMesh.SamplePosition(destination, out navMeshHit, 1.0f, NavMesh.AllAreas))
        {
            return false;
        }

        RaycastHit hit;
        Vector3 directionToTarget = destination - transform.position;

        if (Physics.Raycast(transform.position, directionToTarget.normalized, out hit, directionToTarget.magnitude, LayerMask.GetMask("Wall")))
        {
            return false;
        }
        return true;
    }
}