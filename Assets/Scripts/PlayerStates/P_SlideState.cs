
using UnityEngine;

public class P_SlideState : P_BaseState
{
    public override void Enter(PlayerStateContext context)
    {
        context.Animator.SetTrigger(context.Slide);
        context.isSliding = true;
        context.PlayerEffects.EnableSlideParticle();
    }
    public override void Update(PlayerStateContext context)
    {
        if (context.isSliding) return;
        context.PlayerEffects.DisableSlideParticle();
        context.SwitchState(context.runState); // After Sliding Switching State to Running
    }
    public override void Exit(PlayerStateContext context)
    {
        context.direction.z = context.forwardSpeed; // Reseting Forward Speed 
    }
}
