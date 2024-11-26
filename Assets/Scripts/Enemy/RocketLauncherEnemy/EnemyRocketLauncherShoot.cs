using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocketLauncherShoot : MonoBehaviour
{
    public GameObject rocket;
    public Transform spawnPoint;
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
        if (isCoolingDown || stopFire)
        {
            return;
        }

        GameObject newObject = Instantiate(rocket, spawnPoint.position, Quaternion.identity);

        Vector3 fireDirectionCasual = CalculateFireDirection("Casual");

        if (fireDirectionCasual == Vector3.forward)
        {
            fireDirectionCasual = CalculateFireDirection("PlayerModel");
        }

        newObject.transform.rotation = Quaternion.LookRotation(fireDirectionCasual);

        newObject.GetComponent<Rigidbody>().velocity = fireDirectionCasual.normalized * 18f;

        GetComponent<AudioSource>().Play();

        cooldownTimer = cooldownTime;
        isCoolingDown = true;
    }

    Vector3 CalculateFireDirection(string tag)
    {
        GameObject target = FindClosestTargetWithTag(tag);
        if (target != null)
        {
            return (target.transform.position - spawnPoint.position).normalized;
        }
        else
        {
            return Vector3.forward;
        }
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