using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasualPistol : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public LineRenderer bulletLine;
    public LayerMask shootableLayer;

    private GameObject targetEnemy;
    public CasualAI casualAI;

    public float bulletSpeed;
    public float bulletLineDuration;
    public float cooldownTime;
    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;
    public float bulletMultiplier;
    public float bulletSpread;
    public float bulletDamageAmount;

    void Update()
    {
        if (casualAI != null)
        {
            targetEnemy = casualAI.targetEnemy;
        }

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
        if (!isCoolingDown && targetEnemy != null)
        {
            Vector3 fireDirection = (targetEnemy.transform.position - muzzlePoint.position).normalized;

            int numBullets = Mathf.CeilToInt(bulletMultiplier);
            for (int i = 0; i < numBullets; i++)
            {
                Vector3 spreadDirection = fireDirection;
                spreadDirection.x += Random.Range(-bulletSpread, bulletSpread);
                spreadDirection.y += Random.Range(-bulletSpread, bulletSpread);
                spreadDirection.z += Random.Range(-bulletSpread, bulletSpread);
                spreadDirection.Normalize();

                GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity = spreadDirection * bulletSpeed;

                bullet.GetComponent<PistolBullet>().damageAmount = bulletDamageAmount;

                Destroy(bullet, 1f);
            }

            GetComponent<AudioSource>().Play();

            StartCoroutine(PlayMuzzleFlash());
            DisplayLine();

            cooldownTimer = cooldownTime;
            isCoolingDown = true;
        }
    }

    private void DisplayLine()
    {
        bulletLine.enabled = true;

        if (targetEnemy != null)
        {
            Vector3 fireDirection = (targetEnemy.transform.position - muzzlePoint.position).normalized;
            if (Physics.Raycast(muzzlePoint.position, fireDirection, out RaycastHit hit, Mathf.Infinity, shootableLayer))
            {
                bulletLine.SetPosition(1, hit.point);
            }
            else
            {
                bulletLine.SetPosition(1, targetEnemy.transform.position);
            }

            StartCoroutine(HideLineAfterDuration());
        }
    }

    private void UpdateBulletLine()
    {
        bulletLine.SetPosition(0, muzzlePoint.position);
    }

    private IEnumerator HideLineAfterDuration()
    {
        yield return new WaitForSeconds(bulletLineDuration);
        bulletLine.enabled = false;
    }

    private IEnumerator PlayMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }
}