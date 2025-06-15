using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("TimeSlip Settings")]
    public float slipThreshold = 80f; // Pixels
    public float slowTimeScale = 0.3f;
    public float normalTimeScale = 1f;

    [Header("Time Fuel")]
    public float maxFuel = 100f;
    public float currentFuel = 100f;
    public float fuelDrainRate = 25f;    // units per second
    public float fuelRechargeRate = 15f; // units per second
    private bool timeSlipping = false;

    private Vector2 initialTouchPos;
    private Vector2 currentTouchPos;
    private Camera mainCam;
    private TimeFuelUI fuelUI;
    private TimeSlipVisual timeSlipVisual;

    void Start()
    {
        mainCam = Camera.main;
        Time.timeScale = normalTimeScale;
        fuelUI = FindFirstObjectByType<TimeFuelUI>();
        timeSlipVisual = FindFirstObjectByType<TimeSlipVisual>();
    }

    void Update()
    {
        // Always move upward
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);

        // Handle directional input + TimeSlip logic
        HandleTouchInput();
        ClampToViewport();
        ManageFuel(); // NEW
    }

    void ClampToViewport()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

   void HandleTouchInput()
{
#if UNITY_EDITOR
    if (Input.GetMouseButtonDown(0))
    {
        initialTouchPos = Input.mousePosition;
    }
    else if (Input.GetMouseButton(0))
    {
        currentTouchPos = Input.mousePosition;

        Vector2 dragVector = currentTouchPos - initialTouchPos;
        float dragDistance = dragVector.magnitude;
        Vector2 moveDirection = dragVector.normalized;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            if (dragDistance > slipThreshold && currentFuel > 0)
            {
                timeSlipping = true;
                Time.timeScale = slowTimeScale;
                timeSlipVisual?.ShowEffect();
            }
            else
            {
                timeSlipping = false;
                Time.timeScale = normalTimeScale;
                timeSlipVisual?.HideEffect();
            }
    }
    else if (Input.GetMouseButtonUp(0))
    {
        timeSlipping = false;
        Time.timeScale = normalTimeScale;
    }
#else
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        Vector2 screenPos = touch.position;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                initialTouchPos = screenPos;
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                currentTouchPos = screenPos;
                Vector2 dragVector = currentTouchPos - initialTouchPos;
                float dragDistance = dragVector.magnitude;
                Vector2 moveDirection = dragVector.normalized;

                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                if (dragDistance > slipThreshold && currentFuel > 0)
                {
                    timeSlipping = true;
                    Time.timeScale = slowTimeScale;
                    timeSlipVisual?.ShowEffect();
                }
                else
                {
                    timeSlipping = false;
                    Time.timeScale = normalTimeScale;
                    timeSlipVisual?.HideEffect();
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                timeSlipping = false;
                Time.timeScale = normalTimeScale;
                timeSlipVisual?.HideEffect();
                break;
        }
    }
    else
    {
        timeSlipping = false;
        Time.timeScale = normalTimeScale;
    }
#endif
}


    void ManageFuel()
    {
        Debug.Log(currentFuel);
        if (timeSlipping && currentFuel > 0)
        {
            currentFuel -= fuelDrainRate * Time.unscaledDeltaTime;
            currentFuel = Mathf.Max(0f, currentFuel);
        }
        else if (!timeSlipping)
        {
            currentFuel += fuelRechargeRate * Time.unscaledDeltaTime;
            currentFuel = Mathf.Min(maxFuel, currentFuel);
        }

        //Update fuel bar UI
        if (fuelUI != null)
        {
            fuelUI.SetFuel(currentFuel / maxFuel); // normalized value between 0 and 1
        }
    }
}
