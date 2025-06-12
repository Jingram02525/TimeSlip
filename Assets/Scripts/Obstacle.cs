using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // Destroy if off screen (cleanup)
        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
