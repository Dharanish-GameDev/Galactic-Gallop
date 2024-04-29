using System;
using UnityEngine;
[DefaultExecutionOrder(-1)]
public class Swipe_Deduction : MonoBehaviour
{
    public static Swipe_Deduction Instance { get; private set; }
    public static Action LeftSwipe;
    public static Action RightSwipe;
    public static Action UpSwipe;
    public static Action DownSwipe;


    #region Variables
    public static bool tap;
    public bool  swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;
    private Vector2 startTouch ,swipeDelta;
    #endregion

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        tap = swipeDown = swipeUp = swipeLeft = swipeRight = false;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            ResetInput();
        }
        #endregion

        #region Mobile Input
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                ResetInput();
            }
        }
        #endregion

        //Calculate the distance
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length < 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        //Did we cross the distance?
        if (swipeDelta.magnitude > 12f) 
        {
            //Which direction?
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                //Left or Right
                if (swipeDelta.x < 0)
                {
                    //LeftSwipe?.Invoke();
                    swipeLeft = true;
                }
                    
                else
                {
                    //RightSwipe?.Invoke();
                    swipeRight = true;
                }   
            }
            else
            {
                //Up or Down
                if (swipeDelta.y < 0)
                {
                    //DownSwipe?.Invoke();
                    swipeDown = true;
                }
                else
                {
                    //UpSwipe?.Invoke();
                    swipeUp = true;
                }
            }
            ResetInput();
        }

    }
    private void ResetInput() // Reset Draging
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
}
