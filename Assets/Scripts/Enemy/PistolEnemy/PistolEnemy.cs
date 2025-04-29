using System.Collections;
using UnityEngine;

public class PistolEnemy : MonoBehaviour
{
    private Transform player;
    public GameObject muzzleFlash;
    public GameObject bulletPrefab;
    public Transform muzzlePoint;

    public LayerMask shootableLayer;
    public float bulletSpeed = 50f;
    public bool stopFire;

    public float fireRate;
    private float lastFireTime = 0f;

    void Start()
    {
        player = GameObject.Find("PlayerModel").transform;
    }

    public void Fire()
    {
        if (Time.time - lastFireTime >= fireRate)
        {
            lastFireTime = Time.time;
            GetComponent<AudioSource>().Play();
            StartCoroutine(PlayMuzzleFlash());
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Vector3 fireDirection = CalculateFireDirection();
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
 
        rb.linearVelocity = fireDirection * bulletSpeed;
    }

    Vector3 CalculateFireDirection()
    {
        RaycastHit hit;
        Vector3 fireDirection;

        if (Physics.Raycast(muzzlePoint.position, player.position - muzzlePoint.position, out hit, Mathf.Infinity, shootableLayer))
        {
            fireDirection = (hit.point - muzzlePoint.position).normalized;
        }
        else
        {
            fireDirection = (player.position - muzzlePoint.position).normalized;
        }

        return fireDirection;
    }

    IEnumerator PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            muzzleFlash.SetActive(false);
        }
    }
}