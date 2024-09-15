using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackState : PlayerAbilityState
{
    public PlayerKnockbackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.ChangeLayer();
        Combat?.Knockback(playerData.knockbackAngle, playerData.knockbackStrength, -Movement.FacingDirection);

    }
    public override void Exit()
    {
        base.Exit();
        Combat?.ResetKnockbackState();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isGrounded)
        {
            player.Anim.SetBool("knockbackOnGround", true);
            Movement?.SetVelocityZero();
            player.StartCoroutine(ResetKnockbackOnGround());
        }
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
        player.ResetLayer();
        if(PlayerStats.Health.CurrentValue <= 0)
        {
            stateMachine.ChangeState(player.DeathState);
        }
    }
    private IEnumerator ResetKnockbackOnGround()
    {
        yield return new WaitForSeconds(.2f);
        player.Anim.SetBool("knockbackOnGround", false);

    }
}
