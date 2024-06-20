using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    private void Update()
    {
        transform.position = this.gameObject.transform.position;
    }
}