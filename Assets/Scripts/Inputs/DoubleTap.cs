using System;
using UnityEngine;

public class DoubleTap : MonoBehaviour
{
    public static event Action OnDoubleTap;

    [SerializeField] private float doubleTapTimeThreshold = 0.5f; // Adjust as needed
    [SerializeField] private float maxTapDistance = 50f; // Maximum distance allowed between taps

    private float lastTapTime;
    private Vector2 lastTapPosition;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            if (touch.phase == TouchPhase.Began)
            {
                CheckForDoubleTap(touch.position);
            }
        }
    }

    void CheckForDoubleTap(Vector2 tapPosition)
    {
        if (Time.time - lastTapTime < doubleTapTimeThreshold &&
            Vector2.Distance(tapPosition, lastTapPosition) <= maxTapDistance)
        {
            // Reset the timer for subsequent double tap checks
            lastTapTime = 0f;

            OnDoubleTap?.Invoke();
        }
        else
        {
            // Record the time and position of this tap for potential double tap
            lastTapTime = Time.time;
            lastTapPosition = tapPosition;
        }
    }
}
