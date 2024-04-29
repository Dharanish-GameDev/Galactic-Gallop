using UnityEngine;

public class P_RunState : P_BaseState
{
    public override void Enter(PlayerStateContext context)
    {
        Input_Manager.OnJump += Event_OnJump;
        Input_Manager.OnSlide += Event_OnSlide;  //Subscribing to Input Events
    }
    public override void Update(PlayerStateContext context)
    {
        context.SpeedOverTime();
    }
    public override void Exit(PlayerStateContext context)
    {
       
    }

    //Event Functions
    private void Event_OnSlide(PlayerStateContext context) // Checking Slide For Slide Event
    {
        CheckSlide(context);
    }
    private void Event_OnJump(PlayerStateContext context)  // Checking Jump for Jump Event
    {
        CheckJump(context);
    }

    // Private Functions

    /// <summary>
    /// Checking IsGrounded And Switch State to Slide
    /// </summary>
    /// <param name="context"></param>
    private void CheckSlide(PlayerStateContext context)
    {
        if (!context.CharacterController.isGrounded) return;
        context.SwitchState(context.slideState);
    }
    /// <summary>
    /// Checking IsGrounded And Switch State to Jump
    /// </summary>
    /// <param name="context"></param>
    private void CheckJump(PlayerStateContext context) 
    {
        if (!context.CharacterController.isGrounded) return; 
        context.SwitchState(context.jumpState);
    }
}
