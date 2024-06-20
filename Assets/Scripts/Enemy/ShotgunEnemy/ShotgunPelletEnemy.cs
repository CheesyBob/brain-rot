using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPelletEnemy : MonoBehaviour
{
    public float speed; 
    private float changeDirectionInterval = 2f;
    private float timer = 0f;
    private float targetDirection;

    void Start()
    {
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
}