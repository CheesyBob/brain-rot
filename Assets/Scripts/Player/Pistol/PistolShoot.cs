using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PistolShoot : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletPrefab;
    public TextMeshProUGUI ammoText;

    public AudioClip noAmmoSound;

    private Camera mainCamera;

    public LayerMask shootableLayer;

    public Transform bulletSpawnPoint;
    public float yRotationOffset = 0f;

    public float fireRate;
    public float bulletSpeed;
    public float bulletDuration;

    public float bulletMultiplier;
    public float bulletSpread;
    public float bulletDamageAmount;
    private int removeModifier = 2;
    public bool useAmmo;
    public int currentAmmo;
    public bool ableToShoot;

    private float nextFireTime;

    private void Start()
    {
        muzzleFlash.SetActive(false);
        mainCamera = DestroyClonedPlayer.Instance.playerCamera.GetComponent<Camera>();
    }

    void OnEnable()
    {
        muzzleFlash.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (int.TryParse(ammoText.text, out currentAmmo))
        {
            UpdateText();
        }
        AmmoCheck();
    }

    public void Shoot()
    {
        if ((!useAmmo || ableToShoot))
        {
            StartCoroutine(PlayMuzzleFlash());
            GetComponent<AudioSource>().Play();
            SpawnBullets();
            
            if (useAmmo)
            {
                RemoveAmmo();
            }
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(noAmmoSound);
        }
    }

    IEnumerator PlayMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    private void SpawnBullets()
    {
        for (int i = 0; i < bulletMultiplier; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<PistolBullet>().damageAmount = bulletDamageAmount;

            MoveTowardsMouseWithSpread(bullet);
            Destroy(bullet, bulletDuration);
        }
    }

    private void MoveTowardsMouseWithSpread(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 direction = mousePosition - bullet.transform.position;

        direction.y = 0f;
        direction.Normalize();

        Vector3 spreadDirection = direction + new Vector3(
            Random.Range(-bulletSpread, bulletSpread),
            0f,
            Random.Range(-bulletSpread, bulletSpread)
        );

        spreadDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(spreadDirection.x, 0f, spreadDirection.z));
        rb.MoveRotation(targetRotation);

        rb.linearVelocity = spreadDirection * bulletSpeed;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
    }

    private void UpdateText()
    {
        ammoText.text = currentAmmo.ToString();
    }

    private void RemoveAmmo()
    {
        if (currentAmmo >= 0)
        {
            currentAmmo -= removeModifier;
            UpdateText();
        }
    }

    private void AmmoCheck()
    {
        if (useAmmo)
        {
            ableToShoot = currentAmmo > 0;
            if (currentAmmo <= 0)
            {
                ammoText.text = "0";
            }
        }
        else
        {
            ableToShoot = true;
        }
    }
}