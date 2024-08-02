using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolySlashState : PlayerAttackState
{
    public PlayerHolySlashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = 0;
    }

    protected override int AttackCounter => 0;

    protected override AttackDetails AttackDetails => playerData.holySlashAttack;
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityZero();

        if (player.InputHandler.AttackInputStop)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(player.IdleState);
    }
}
