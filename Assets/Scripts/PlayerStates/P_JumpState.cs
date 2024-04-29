using UnityEngine;

public class P_JumpState : P_BaseState
{
    private bool jumpToSlide = false;
    public override void Enter(PlayerStateContext context)
    {
        context.Animator.SetTrigger(context.Jump);
        context.isJumping = true;
        context.direction.y = context.JumpForce;

        context.PlayerEffects.EnableJumpParticle();
        context.PlayerEffects.DisableSlideParticle();
        jumpToSlide = false;
        Input_Manager.OnSlide += InputManager_OnSlide;
    }

    private void InputManager_OnSlide(PlayerStateContext obj)
    {
        jumpToSlide = true;
    }

    public override void Update(PlayerStateContext context)
    {
        //if (context.CharacterController.isGrounded) // Returning When Player is Grounded
        //    return;

        if (!context.isJumping) // Switching state to Running After jump
        {
            context.SwitchState(context.runState);
        }

        else if (jumpToSlide && CanSlideUsingRaycast(context))  // Switching state to Slide for Jump Slide
        {
            context.SwitchState(context.slideState);
        }
        if(context.isJumping)
        {
            context.direction.y += context.Gravity * 1.4f * Time.deltaTime;
        }
        if(jumpToSlide)
        {
            context.direction.y += context.JumpForce * -3.5f;
        }
    }
    public override void Exit(PlayerStateContext context) 
    {
        context.PlayerEffects.DisableJumpParticle();
        context.isJumping = false;
    }
    private bool CanSlideUsingRaycast(PlayerStateContext context)  // Return a bool for Switching State to Slide using Rayacst
    {
        return Physics.CheckSphere(context.GroundCheckPos.position, 0.7f, context.GroundCheckLayer);
    }
}
