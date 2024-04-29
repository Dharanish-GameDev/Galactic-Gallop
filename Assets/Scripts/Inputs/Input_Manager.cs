
using System;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{ 
    #region Events of Inputs

    public static event Action<PlayerStateContext> OnJump;
    public static event Action<PlayerStateContext> OnSlide;
    public static event Action OnLeftSwitch;
    public static event Action OnRightSwitch;

    #endregion


    private void Update()
    {
        if(Swipe_Deduction.Instance.swipeUp)
        {
            OnJump?.Invoke(PlayerStateContext.Instance);  // Invoking Jump Event
        }
        if(Swipe_Deduction.Instance.swipeDown)
        {
            OnSlide?.Invoke(PlayerStateContext.Instance); // Invoking Slide Event
        }
        if(Swipe_Deduction.Instance.swipeLeft)
        {
            OnLeftSwitch?.Invoke();
        }
        if(Swipe_Deduction.Instance.swipeRight)
        {
           OnRightSwitch?.Invoke();
        }

    }
}
