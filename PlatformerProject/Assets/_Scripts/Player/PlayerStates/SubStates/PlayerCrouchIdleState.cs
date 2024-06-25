using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    private bool slideInput;

    public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityZero();
        player.SetColliderHeight(playerData.crouchColliderHeight);

        Movement.RB.sharedMaterial = playerData.fullFriction;

    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.standColliderHeight);

        Movement.RB.sharedMaterial = playerData.noFriction;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        slideInput = player.InputHandler.SlideInput;

        if (!isExitingState)
        {
            if (yInput != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (slideInput && player.SlideState.CheckIfCanSlide())
            {
                stateMachine.ChangeState(player.SlideState);
            }
        }
    }
}