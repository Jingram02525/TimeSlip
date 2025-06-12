using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 1.5f;
    public float spawnXMargin = 0.5f; // Padding from screen edges
    public float fallSpeed = 3f;

    private float timer = 0f;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        float camHeight = 2f * mainCam.orthographicSize;
        float camWidth = camHeight * mainCam.aspect;

        float minX = mainCam.transform.position.x - camWidth / 2f + spawnXMargin;
        float maxX = mainCam.transform.position.x + camWidth / 2f - spawnXMargin;
        float spawnX = Random.Range(minX, maxX);
        float spawnY = mainCam.transform.position.y + camHeight / 2f + 1f; // Just above view

        Vector2 spawnPos = new Vector2(spawnX, spawnY);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * fallSpeed;
        }
    }
}
