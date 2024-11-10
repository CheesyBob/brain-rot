using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PistolShoot : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletPrefab;

    private Camera mainCamera;

    public LineRenderer bulletLine;

    public LayerMask shootableLayer;

    public float fireRate;
    public float bulletSpeed;
    public float bulletDuration;
    public float lineDuration;
    private float nextFireTime;

    public Vector3 distanceFromGun;

    private void Start()
    {
        muzzleFlash.SetActive(false);

        bulletLine.enabled = false;

        mainCamera = DestroyClonedPlayer.Instance.playerCamera.GetComponent<Camera>();
    }

    void OnEnable(){
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
    }

    public void Shoot()
    {
        Vector3 startPoint = GetGunEndPosition();
        Vector3 endPoint = GetMouseWorldPosition();
        
        endPoint.y = startPoint.y;

        StartCoroutine("PlayMuzzleFlash");

        GetComponent<AudioSource>().Play();

        DisplayLine(startPoint, endPoint);
        ShootBullet(startPoint, endPoint);
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
        GameObject bullet = Instantiate(bulletPrefab, start, Quaternion.identity);
        Vector3 direction = (end - start).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.velocity = direction * bulletSpeed;

        GetComponent<AudioSource>().Play();

        Ray ray = new Ray(start, direction);

        Destroy(bullet, bulletDuration);
    }
}