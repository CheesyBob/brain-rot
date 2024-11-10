using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform Player;

    public float distance;
    public float height;

    private Vector3 offset;

    void Start()
    {
        Player = GameObject.Find("PlayerModel").transform;
        
        offset = transform.position - Player.position;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = Player.position + offset.normalized * distance;
        desiredPosition.y = height;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 1000 * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(Player.position);
    }
}