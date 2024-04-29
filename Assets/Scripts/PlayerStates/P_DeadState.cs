using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_DeadState : P_BaseState
{
    public override void Enter(PlayerStateContext context)
    {
        context.isDead = true;
        context.PlayerEffects.DisableSlideParticle();
        context.PlayerEffects.DisableJumpParticle();
        AudioManager.instance.PlaySfx(AudioManager.instance.AudioClips.failSfx, AudioManager.instance.AudioSourcesList.meteorBerakSfxSource, 1);
        AudioManager.instance.FadeGameBgMusic();
    }

    public override void Update(PlayerStateContext context)
    {
        context.Animator.SetTrigger(context.Dead);
        context.direction.y += context.Gravity * Time.deltaTime * 1.2f;
        context.direction.z = 0;
        context.CharacterController.Move(context.direction * Time.deltaTime);
    }

    public override void Exit(PlayerStateContext context)
    {

    }
}
