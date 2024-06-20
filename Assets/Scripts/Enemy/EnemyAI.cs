using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public GameObject Pistol;
    public GameObject ShotgunEnd;
    public GameObject Shotgun;
    public GameObject RocketLauncher;

    private GameObject PlayerPistol;
    private GameObject PlayerShotgun;
    private GameObject PlayerRocketLauncher;

    private Transform playerTransform;
    
    private Vector3 roamPosition;

    public float currentHealth;
    public float playerBulletDamage;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float roamRadius = 5f;
    public float roamInterval = 5f;
    private float nextRoamTime;

    public bool canShoot = true;
    public bool EnemyPistol;
    public bool EnemyShotgun;
    public bool EnemyRocketLauncher;
    private bool isChasingPlayer = false;
    private bool hasAttackedPlayer = false;
    private bool hasSpottedPlayer = false;
    private bool hasSetHealth = false;

    public AudioClip[] spottedPlayerAudioClips;
    private AudioSource audioSource;

    void Start()
    {
        PlayerPistol = DestroyClonedPlayer.Instance.playerPistol;
        PlayerShotgun = DestroyClonedPlayer.Instance.playerShotgun;
        PlayerRocketLauncher = DestroyClonedPlayer.Instance.playerRocketLauncher;

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        playerTransform = GameObject.FindWithTag("PlayerModel").transform;
        
        SetRandomRoamPosition();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if(currentHealth == 0f && !hasSetHealth){
            GetComponent<EnemyDeath>().dead = true;

            hasSetHealth = true;
        }

        if(currentHealth <= 0f && !hasSetHealth){
            GetComponent<EnemyDeath>().dead = true;

            hasSetHealth = true;
        }

        if(distanceToPlayer <= chaseRange)
        {
            if(HasLineOfSightToPlayer()){

                if(EnemyPistol){
                    Pistol.GetComponent<PistolEnemy>().stopFire = false;
                }
                if(EnemyShotgun){
                    ShotgunEnd.GetComponent<ShotgunEnemy>().stopFire = false;
                }
                if(EnemyRocketLauncher){
                    RocketLauncher.GetComponent<EnemyRocketLauncherShoot>().stopFire = false;
                }

                isChasingPlayer = true;

                PlayerPistol.GetComponent<PistolShoot>().shot = false;
                PlayerShotgun.GetComponent<ShotgunShoot>().shot = false;
                PlayerRocketLauncher.GetComponent<RocketLauncherShoot>().shot = false;
            }
            else{
                if(EnemyPistol){
                    Pistol.GetComponent<PistolEnemy>().stopFire = true;
                }
                if(EnemyShotgun){
                    ShotgunEnd.GetComponent<ShotgunEnemy>().stopFire = true;
                }
                if(EnemyRocketLauncher){
                    RocketLauncher.GetComponent<EnemyRocketLauncherShoot>().stopFire = true;
                }
            }
        }
        else
        {
            isChasingPlayer = false;
        }

        if(isChasingPlayer)
        {
            navMeshAgent.SetDestination(playerTransform.position);

            if(!hasSpottedPlayer && canShoot)
            {
                SpottedPlayerAudioClip();

                hasSpottedPlayer = true;
            }

            if(!hasAttackedPlayer){
                if(EnemyPistol && canShoot){
                    Pistol.GetComponent<PistolEnemy>().FirePistol();
                }
                if(EnemyShotgun && canShoot){
                    ShotgunEnd.GetComponent<ShotgunEnemy>().FireShotgun();
                }
                if(EnemyRocketLauncher && canShoot){
                    StartCoroutine("FireRocketDelay");
                }
            }
        }
        else
        {
            if(Time.time >= nextRoamTime)
            {
                SetRandomRoamPosition();

                nextRoamTime = Time.time + roamInterval;
            }
            hasAttackedPlayer = false;
            hasSpottedPlayer = false;

            navMeshAgent.SetDestination(roamPosition);
        }
        if(PlayerPistol.GetComponent<PistolShoot>().shot && distanceToPlayer >= chaseRange){
            navMeshAgent.SetDestination(playerTransform.position);
        }
        if(PlayerShotgun.GetComponent<ShotgunShoot>().shot && distanceToPlayer >= chaseRange){
            navMeshAgent.SetDestination(playerTransform.position);
        }
        if(PlayerRocketLauncher.GetComponent<RocketLauncherShoot>().shot && distanceToPlayer >= chaseRange){
            navMeshAgent.SetDestination(playerTransform.position);
        }

        if(navMeshAgent.velocity.magnitude == 0){
            GetComponent<Animator>().SetBool("isRunning", false);
        }
        else{
            GetComponent<Animator>().SetBool("isRunning", true);
        }
    }

    IEnumerator FireRocketDelay(){
        yield return new WaitForSeconds(.4f);
        RocketLauncher.GetComponent<EnemyRocketLauncherShoot>().FireRocket();
    }

    void SetRandomRoamPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;

        randomDirection += transform.position;

        UnityEngine.AI.NavMeshHit hit;

        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);

        roamPosition = hit.position;
    }

    bool HasLineOfSightToPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit, chaseRange))
        {
            if (hit.collider.CompareTag("PlayerModel"))
            {
                return true;
            }
        }

        return false;
    }

    void SpottedPlayerAudioClip()
    {
        if (spottedPlayerAudioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, spottedPlayerAudioClips.Length);

            audioSource.PlayOneShot(spottedPlayerAudioClips[randomIndex]);
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "PlayerBullet"){
            if(currentHealth >= 0f && !hasSetHealth){
                currentHealth -= playerBulletDamage;

                currentHealth = Mathf.Max(currentHealth, 0);
            }
        }
    }
}