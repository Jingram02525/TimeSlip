using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("TimeSlip Settings")]
    public float slipThreshold = 80f; // Pixels
    public float slowTimeScale = 0.3f;
    public float normalTimeScale = 1f;

    private Vector2 initialTouchPos;
    private Vector2 currentTouchPos;
    private bool isTouching = false;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        Time.timeScale = normalTimeScale;
    }

    void Update()
    {
        // Always move upward
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);

        // Handle directional input + TimeSlip logic
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
#if UNITY_EDITOR
        // Mouse input for testing in the Unity Editor
        if (Input.GetMouseButtonDown(0))
        {
            isTouching = true;
            initialTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            currentTouchPos = Input.mousePosition;

            Vector2 dragVector = currentTouchPos - initialTouchPos;
            float dragDistance = dragVector.magnitude;
            Vector2 moveDirection = dragVector.normalized;

            Vector2 forward = Vector2.up;
            Vector2 combinedMove = (forward + moveDirection).normalized;

            transform.Translate(combinedMove * moveSpeed * Time.deltaTime);

            Time.timeScale = (dragDistance > slipThreshold) ? slowTimeScale : normalTimeScale;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isTouching = false;
            Time.timeScale = normalTimeScale;
        }
#else
        // Touch input for mobile builds
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 screenPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isTouching = true;
                    initialTouchPos = screenPos;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    currentTouchPos = screenPos;
                    Vector2 dragVector = currentTouchPos - initialTouchPos;
                    float dragDistance = dragVector.magnitude;
                    Vector2 moveDirection = dragVector.normalized;

                    Vector2 forward = Vector2.up;
                    Vector2 combinedMove = (forward + moveDirection).normalized;

                    transform.Translate(combinedMove * moveSpeed * Time.deltaTime);

                    Time.timeScale = (dragDistance > slipThreshold) ? slowTimeScale : normalTimeScale;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isTouching = false;
                    Time.timeScale = normalTimeScale;
                    break;
            }
        }
        else
        {
            Time.timeScale = normalTimeScale;
        }
#endif
    }
}
