using System.Collections;
using UnityEngine;
using TMPro;

public class ShotgunBulletEnemy : MonoBehaviour
{
    private GameObject PainPannel;

    public float speed;
    private float changeDirectionInterval = 2f;
    private float timer = 0f;
    private float targetDirection;

    void Start()
    {
        PainPannel = GameObject.Find("PainPannel");

        GenerateNewDirection();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * targetDirection * speed * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer >= changeDirectionInterval)
        {
            GenerateNewDirection();
            timer = 0f;
        }
    }

    void GenerateNewDirection()
    {
        targetDirection = Random.Range(-1f, 1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            other.GetComponent<HealthStatus>().DamagePlayer(6);

            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}