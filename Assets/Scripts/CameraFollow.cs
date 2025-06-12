using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = target.position + offset;
            newPos.z = -10f; // Keep the camera at z = -10
            transform.position = newPos;
        }
    }
}
