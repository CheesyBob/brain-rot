using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolEnemy : MonoBehaviour
{
    private Transform player;
    public GameObject muzzleFlash;
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    
    public LineRenderer bulletLine;
    public LayerMask shootableLayer;

    public float bulletLineDuration;

    public bool stopFire;

    void Start()
    {
        player = GameObject.Find("PlayerModel").transform;
    }

    void Update()
    {
        UpdateBulletLine();
    }

    public void Fire()
    {
        GetComponent<AudioSource>().Play();

        StartCoroutine(PlayMuzzleFlash());
        DisplayLine();
    }

    public void DisplayLine()
    {
        if (bulletLine != null)
        {
            bulletLine.enabled = true;
            Vector3 fireDirection = CalculateFireDirection();

            RaycastHit hit;
            if (Physics.Raycast(muzzlePoint.position, fireDirection, out hit, Mathf.Infinity, shootableLayer))
            {
                bulletLine.SetPosition(1, hit.point);
            }
            else
            {
                bulletLine.SetPosition(1, muzzlePoint.position + fireDirection * 100f);
            }

            StartCoroutine(HideLineAfterDuration());
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

    void UpdateBulletLine()
    {
        bulletLine.SetPosition(0, muzzlePoint.position);
    }

    IEnumerator HideLineAfterDuration()
    {
        yield return new WaitForSeconds(bulletLineDuration);
        bulletLine.enabled = false;
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