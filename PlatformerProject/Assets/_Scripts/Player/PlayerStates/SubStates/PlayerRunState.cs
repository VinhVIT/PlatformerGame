using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundedState
{
    private float runningTime;
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ResetRunningTime();

    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        runningTime += Time.deltaTime;
        if (!runInput)
        {
            stateMachine.ChangeState(player.MoveState);
            ResetRunningTime();
        }
        else if (xInput != 0 && xInput != Movement.FacingDirection)
        {
            stateMachine.ChangeState(player.TurnState);
            ResetRunningTime();
        }
        if (!isExitingState)
        {
            Movement?.CheckIfShouldFlip(xInput);
            Movement?.SetVelocityX(playerData.runVelocity * xInput);
            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
                ResetRunningTime();
            }
        }
    }
    private void ResetRunningTime() => runningTime = 0;
    public bool RunningEnough() => runningTime > playerData.maxRunningTime;
    public void CheckIfShouldSprintJump()
    {
        if (runningTime > playerData.maxRunningTime)
        {
            player.Anim.SetBool("sprintJump", true);
            ResetRunningTime();
        }
    }
}
