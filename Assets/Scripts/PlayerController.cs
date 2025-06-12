using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    [Header("Time Control")]
    public float slowTimeScale = 0.3f;
    public float normalTimeScale = 1f;

    private bool isAlive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = normalTimeScale;
    }

    void Update()
    {
        if (!isAlive) return;

        HandleTimeControl();
        MoveForward();
    }

    void HandleTimeControl()
    {
        if (Input.GetMouseButton(0))
        {
            Time.timeScale = slowTimeScale;
        }
        else
        {
            Time.timeScale = normalTimeScale;
        }
    }

    void MoveForward()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    void Die()
    {
        isAlive = false;
        Time.timeScale = 0f;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Player Died");
    }
}
