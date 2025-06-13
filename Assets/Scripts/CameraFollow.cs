using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Usually player
    public float yOffset = 1.5f;
    public float smoothSpeed = 0.125f;
    public float fixedX = 0f; // Lock X position

    void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 desiredPosition = new Vector3(
            fixedX,
            target.position.y + yOffset,
            transform.position.z
        );

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
