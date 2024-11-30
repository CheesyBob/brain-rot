using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolEnemyBullet : MonoBehaviour
{
    public float BulletSpeed;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.velocity = transform.forward * BulletSpeed;

        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            other.GetComponent<HealthStatus>()?.DamagePlayer(3);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Casual"))
        {
            Destroy(gameObject);
        }
    }
}