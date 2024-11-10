using UnityEngine;

public class ExitArrow : MonoBehaviour
{
    public Transform LevelTrigger;
    private GameObject playerTransform;
    public Vector3 offset;

    public float smoothSpeed;

    void Awake(){
        playerTransform = GameObject.FindGameObjectWithTag("PlayerModel");
    }

    void Update()
    {
        Vector3 direction = LevelTrigger.position - transform.position;
        direction.y = 0f; 

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion rotation = Quaternion.LookRotation(-direction);

            transform.rotation = rotation;
        }
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        transform.position = smoothedPosition;
    }
}