using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShotgunShoot : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletPrefab;

    public TextMeshProUGUI ammoText;

    private Camera mainCamera;

    public LineRenderer bulletLine;

    public LayerMask shootableLayer;

    public AudioClip pistolShootSound;
    public AudioClip noAmmoSound;

    private AudioSource audioSource;

    public float fireRate;
    public float bulletSpeed;
    public float bulletDuration;
    public float lineDuration;
    private float nextFireTime;

    public int pelletModifier;
    private int removeModifier = 1;
    public int currentAmmo;

    public bool ableToShoot;

    public Vector3 distanceFromGun;

    public float bulletSpread;

    private void Start()
    {
        muzzleFlash.SetActive(false);
        bulletLine.enabled = false;

        audioSource = this.gameObject.GetComponent<AudioSource>();
        mainCamera = DestroyClonedPlayer.Instance.playerCamera.GetComponent<Camera>();
    }

    void OnEnable()
    {
        muzzleFlash.SetActive(false);
        bulletLine.enabled = false;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && GameObject.Find("LoadingScreenCanvas").GetComponent<DestroyLoadingScreen>().canShoot)
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
        if (ableToShoot)
        {
            Vector3 startPoint = GetGunEndPosition();
            Vector3 endPoint = GetMouseWorldPosition();

            endPoint.y = startPoint.y;

            StartCoroutine("PlayMuzzleFlash");

            DisplayLine(startPoint, endPoint);
            ShootBullet(startPoint, endPoint);

            audioSource.clip = pistolShootSound;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(noAmmoSound);
        }
    }

    IEnumerator PlayMuzzleFlash()
    {
        muzzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.gameObject.SetActive(false);
    }

    private void DisplayLine(Vector3 start, Vector3 end)
    {
        StartCoroutine(HideLineAfterDuration());
        bulletLine.enabled = true;
        bulletLine.SetPosition(0, start);

        RaycastHit hit;

        if (Physics.Raycast(start, (end - start).normalized, out hit, Vector3.Distance(start, end), shootableLayer))
        {
            bulletLine.SetPosition(1, hit.point);
        }
        else
        {
            bulletLine.SetPosition(1, end);
        }
    }

    private IEnumerator HideLineAfterDuration()
    {
        yield return new WaitForSeconds(lineDuration);
        bulletLine.enabled = false;
    }

    private Vector3 GetGunEndPosition()
    {
        Vector3 endPoint = transform.position + transform.forward * distanceFromGun.z + transform.right * distanceFromGun.x + transform.up * distanceFromGun.y;

        return endPoint;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            return hitPoint;
        }

        return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromGun.z));
    }

    private void ShootBullet(Vector3 start, Vector3 end)
    {
        for (int i = 0; i < pelletModifier; i++)
        {
            Vector3 direction = (end - start).normalized;

            direction = Quaternion.Euler(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), 0) * direction;

            GameObject bullet = Instantiate(bulletPrefab, start, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.velocity = direction * bulletSpeed;

            Destroy(bullet, bulletDuration);
            
            RemoveAmmo();
        }
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
        if (currentAmmo >= 0)
        {
            ableToShoot = true;
        }
        if (currentAmmo <= 0)
        {
            ableToShoot = false;
        }
        if (ammoText.text == "-1")
        {
            ammoText.text = "0";
        }
    }
}