using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlinePosition : MonoBehaviour
{
    public GameObject targetTransform;

    void Update()
    {
        Vector3 direction = targetTransform.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
        FaceMainCamera();
    }

    void LateUpdate()
    {
        Vector3 targetPosition = targetTransform.transform.position;
        targetPosition.y += 3.8f;
        transform.position = targetPosition;
    }

    void FaceMainCamera()
    {
        if (Camera.main != null)
        {
            Vector3 cameraDirection = Camera.main.transform.position - transform.position;
            cameraDirection.y = -2f;

            if (cameraDirection.sqrMagnitude > 0.001f)
            {
                Quaternion cameraRotation = Quaternion.LookRotation(-cameraDirection);
                transform.rotation = cameraRotation;
            }
        }
    }
}