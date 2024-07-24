using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;
    private bool dashInput;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityZero();
        player.SetGravity(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        dashInput = player.InputHandler.DashInput;

        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));

        if (dashInput && player.DashState.CheckIfCanDash())
        {
            player.ResetGravity();
            stateMachine.ChangeState(player.DashState);
        }
        if (Time.time >= startTime + playerData.wallJumpTime && Movement.CurrentVelocity.y < 0)
        {
            isAbilityDone = true;
        }
    }
    private void WallJump()
    {
        player.ResetGravity();
        player.InputHandler.UseJumpInput();
        player.JumpState.ResetAmountOfJumpsLeft();
        Movement?.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        Movement?.CheckIfShouldFlip(wallJumpDirection);
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }
    public override void AnimationStartTrigger()
    {
        base.AnimationStartTrigger();
        WallJump();
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -Movement.FacingDirection;
        }
        else
        {
            wallJumpDirection = Movement.FacingDirection;
        }
    }
}