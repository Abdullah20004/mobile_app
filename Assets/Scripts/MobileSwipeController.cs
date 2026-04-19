using UnityEngine;

public class MobileSwipeController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;

    [Header("Settings")]
    public float minSwipeDistance = 50f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fingerUpPos = Input.mousePosition;
            fingerDownPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            fingerDownPos = Input.mousePosition;
            DetectSwipe();
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPos = touch.position;
                fingerDownPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPos = touch.position;
                DetectSwipe();
            }
        }
    }

    void DetectSwipe()
    {
        float verticalDistance = fingerDownPos.y - fingerUpPos.y;
        float horizontalDistance = fingerDownPos.x - fingerUpPos.x;

        if (Mathf.Abs(verticalDistance) > minSwipeDistance || Mathf.Abs(horizontalDistance) > minSwipeDistance)
        {
            if (Mathf.Abs(verticalDistance) > Mathf.Abs(horizontalDistance))
            {
                if (verticalDistance > 0) playerMovement.TriggerJump();
                else playerMovement.TriggerSlide();
            }
            else
            {
                if (horizontalDistance > 0) playerMovement.TriggerRight();
                else playerMovement.TriggerLeft();
            }
        }
    }
}