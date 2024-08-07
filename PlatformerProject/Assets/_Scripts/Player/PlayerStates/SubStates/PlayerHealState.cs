using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealState : PlayerGroundedState
{

    public PlayerHealState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Combat.OnBeingAttacked += Combat_OnBeingAttacked;
    }

    private void Combat_OnBeingAttacked()
    {
        stateMachine.ChangeState(player.InAirState);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityZero();

        if (player.InputHandler.HealInputStop)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        PlayerStats.Health.Increase(playerData.healingAmount);
        stateMachine.ChangeState(player.IdleState);
    }
}
