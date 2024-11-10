using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollowPlayer : MonoBehaviour
{
    private GameObject playerTransform;
    public float yOffset;

    public void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("PlayerModel");
    }

    public void Update()
    {
        transform.position = playerTransform.transform.position + new Vector3(0, yOffset, 0);
    }
}