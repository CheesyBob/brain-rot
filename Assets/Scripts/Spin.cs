using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float spinSpeed;
    public float bounceHeight;
    public float bounceSpeed;

    private Vector3 initialPosition;
    private float timeElapsed = 0f;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

        timeElapsed += Time.deltaTime * bounceSpeed;
        float newYPosition = initialPosition.y + Mathf.Sin(timeElapsed) * bounceHeight;
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }
}