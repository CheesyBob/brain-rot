using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolEnemy : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletPrefab;

    private Transform player;
    public Transform muzzlePoint;
    public Transform enemyPistol;

    public LineRenderer bulletLine;
    public LayerMask shootableLayer;

    private AudioSource audioSource;
    public AudioClip pistolShootSound;

    public float bulletSpeed;
    public float bulletLineDuration;

    public float cooldownTime;
    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;
    public bool stopFire;

    void Start(){
        player = GameObject.Find("PlayerModel").transform;
        
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isCoolingDown)
        {
            if (cooldownTimer <= 0f)
            {
                isCoolingDown = false;
                cooldownTimer = 0f;
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
            }
        }
        UpdateBulletLine();
    }

    public void FirePistol()
    {
        if(isCoolingDown){
            return;
        }

        if(stopFire){
            return;
        }

        Vector3 fireDirection = CalculateFireDirection();

        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.linearVelocity = fireDirection * bulletSpeed;
        Destroy(bullet, 1f);

        audioSource.clip = pistolShootSound;
        audioSource.Play();

        StartCoroutine("PlayMuzzleFlash");
        DisplayLine();
    }

    public void DisplayLine(){
        bulletLine.enabled = true;
        
        Vector3 fireDirection = CalculateFireDirection();

        if(Physics.Raycast(muzzlePoint.position, fireDirection, out RaycastHit hit, shootableLayer)){
            bulletLine.SetPosition(1, hit.point);
        }
        else{
            bulletLine.SetPosition(1, player.position);
        }

        if (bulletLine != null)
        {
            StartCoroutine(HideLineAfterDuration());

            cooldownTimer = cooldownTime;
            isCoolingDown = true;
        }
    }

    Vector3 CalculateFireDirection()
    {
        RaycastHit hit;
        Vector3 fireDirection;

        if (Physics.Raycast(muzzlePoint.position, player.position - muzzlePoint.position, out hit, shootableLayer))
        {
            fireDirection = (hit.point - muzzlePoint.position).normalized;
        }
        else
        {
            fireDirection = (player.position - muzzlePoint.position).normalized;
        }

        return fireDirection;
    }

    void UpdateBulletLine(){
        bulletLine.SetPosition(0, muzzlePoint.position);
    }

    IEnumerator HideLineAfterDuration(){
        yield return new WaitForSeconds(bulletLineDuration);
        bulletLine.enabled = false;
    }

    IEnumerator PlayMuzzleFlash()
    {
        muzzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.gameObject.SetActive(false);
    }
}