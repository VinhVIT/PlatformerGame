using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        playerData.moveTimeElapsed = 0f;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.SetVelocityX(playerData.movementVelocity * xInput);
        if (!isExitingState)
        {
            if (xInput == 0 && playerData.moveTimeElapsed >= playerData.moveTimeThreshold)
            {
                stateMachine.ChangeState(player.TurnState);
                return;
            }

            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
                playerData.moveTimeElapsed = 0f;
            }
            else if (xInput != 0)
            {
                playerData.moveTimeElapsed += Time.deltaTime;
                if (playerData.moveTimeElapsed >= playerData.moveTimeThreshold && xInput != Movement.FacingDirection)
                {
                    stateMachine.ChangeState(player.TurnState);
                }
                else if (playerData.moveTimeElapsed < playerData.moveTimeThreshold && xInput != Movement.FacingDirection)
                {
                    playerData.moveTimeElapsed = 0f;
                    Movement?.CheckIfShouldFlip(xInput);
                }
            }
            else if (yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}