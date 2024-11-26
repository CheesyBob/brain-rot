using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasualRocketLauncher : MonoBehaviour
{
    public GameObject rocket;
    public Transform spawnPoint;
    public LayerMask shootableLayer;
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
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCoolingDown = false;
                cooldownTimer = 0f;
            }
        }
    }

    public void FireRocket()
    {
        if (isCoolingDown || stopFire)
        {
            return;
        }

        GameObject target = FindClosestTargetWithTag("Enemy");
        if (target == null) return;

        Vector3 fireDirection = (target.transform.position - spawnPoint.position).normalized;

        GameObject newObject = Instantiate(rocket, spawnPoint.position, Quaternion.LookRotation(fireDirection));
        newObject.GetComponent<Rigidbody>().velocity = fireDirection * 18f;

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