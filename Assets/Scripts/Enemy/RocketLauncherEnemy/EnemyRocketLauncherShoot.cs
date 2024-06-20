using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocketLauncherShoot : MonoBehaviour
{
    public GameObject rocket;

    public Transform spawnPoint;

    public LayerMask shootableLayer;

    public AudioClip rocketFireSound;

    public float radius;

    public int damageAmount;

    public float cooldownTime;
    private float cooldownTimer = 0f;

    private bool isCoolingDown = false;
    public bool stopFire;

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
    }

    public void FireRocket()
    {
        if(isCoolingDown){
            return;
        }

        if(stopFire){
            return;
        }

        GameObject newObject = Instantiate(rocket, spawnPoint.position, Quaternion.identity);

        Vector3 fireDirection = CalculateFireDirection();

        newObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        newObject.GetComponent<Rigidbody>().linearVelocity = fireDirection * 18f;

        newObject.GetComponent<AudioSource>().PlayOneShot(rocketFireSound);

        cooldownTimer = cooldownTime;
        isCoolingDown = true;
    }

    Vector3 CalculateFireDirection()
    {
        RaycastHit hit;
        Vector3 fireDirection;

        if (Physics.Raycast(spawnPoint.position, GameObject.Find("PlayerModel").transform.position - spawnPoint.position, out hit, shootableLayer))
        {
            fireDirection = (hit.point - spawnPoint.position).normalized;
        }
        else
        {
            fireDirection = (GameObject.Find("PlayerModel").transform.position - spawnPoint.position).normalized;
        }

        return fireDirection;
    }
}