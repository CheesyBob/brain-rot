using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathwadShoot : MonoBehaviour
{
    public GameObject rocket;
    public Transform spawnPoint;

    private DeathwadFireAmount deathwadFireAmount;
    private Camera playerCamera;

    public AudioClip rocketFireSound;
    public AudioClip noAmmoSound;

    public float spawnForce;

    void Start()
    {
        deathwadFireAmount = FindObjectOfType<DeathwadFireAmount>();
        playerCamera = DestroyClonedPlayer.Instance.playerCamera.GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && deathwadFireAmount.CanShoot() && GameObject.Find("LoadingScreenCanvas").GetComponent<DestroyLoadingScreen>().canShoot)
        {
            SpawnNewObject();
        }

        if (Input.GetMouseButtonDown(0) && !deathwadFireAmount.CanShoot())
        {
            GetComponent<AudioSource>().PlayOneShot(noAmmoSound);
        }
    }

    public void SpawnNewObject()
    {
        GameObject newObject = Instantiate(rocket, spawnPoint.position, Quaternion.identity);

        newObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        newObject.GetComponent<AudioSource>().PlayOneShot(rocketFireSound);

        RemoveAmmo();

        MoveTowardsMouse(newObject);
    }

    void MoveTowardsMouse(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 direction = mousePosition - obj.transform.position;

        direction.y = 0f;
        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        rb.MoveRotation(targetRotation);

        rb.AddForce(direction * spawnForce, ForceMode.Impulse);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            return hitPoint;
        }

        return playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
    }

    private void RemoveAmmo()
    {
        deathwadFireAmount.DecreaseAmmo(deathwadFireAmount.rocketAmmoText);
        deathwadFireAmount.DecreaseAmmo(deathwadFireAmount.molotovAmmoText);
        deathwadFireAmount.UpdateDeathwadAmmo();
    }
}