using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockCounterState : PlayerAttackState
{
    public PlayerBlockCounterState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = 0;
    }
    protected override int AttackCounter => 0;

    protected override AttackDetails AttackDetails => playerData.blockCounterAttackDetails[attackCounter];
    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0;
        ParticleManager?.StartParticle(playerData.blockFX);

    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityZero();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        Movement.CanSetVelocity = true;

    }
    public override void AnimationTrigger()
    {
        Time.timeScale = 1f;//reset Timescale before knockback

        base.AnimationTrigger();
    }
}
