using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocketLauncherShoot : MonoBehaviour
{
    public GameObject rocket;
    public Transform spawnPoint;
    public float cooldownTime;
    private float maxRange = 20f;

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
        if (isCoolingDown || stopFire)
        {
            return;
        }

        Vector3 fireDirection = Vector3.forward;

        GameObject playerTarget = FindClosestTargetWithTag("PlayerModel");
        GameObject casualTarget = FindClosestTargetWithTag("Casual");

        float playerDistance = playerTarget != null 
            ? Vector3.Distance(spawnPoint.position, playerTarget.transform.position) 
            : Mathf.Infinity;
        float casualDistance = casualTarget != null 
            ? Vector3.Distance(spawnPoint.position, casualTarget.transform.position) 
            : Mathf.Infinity;

        if (playerDistance <= maxRange)
        {
            fireDirection = (playerTarget.transform.position - spawnPoint.position).normalized;
        }
        else if (casualDistance <= maxRange)
        {
            fireDirection = (casualTarget.transform.position - spawnPoint.position).normalized;
        }
        else
        {
            return;
        }

        GameObject newObject = Instantiate(rocket, spawnPoint.position, Quaternion.identity);

        Quaternion directionRotation = Quaternion.LookRotation(fireDirection);
        Quaternion offsetRotation = Quaternion.Euler(90f, 0f, 0f);
        
        newObject.transform.rotation = directionRotation * offsetRotation;
        
        newObject.GetComponent<Rigidbody>().velocity = fireDirection.normalized * 18f;

        GetComponent<AudioSource>().Play();

        cooldownTimer = cooldownTime;
        isCoolingDown = true;
    }

    GameObject FindClosestTargetWithTag(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(spawnPoint.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }
        return closestTarget;
    }
}