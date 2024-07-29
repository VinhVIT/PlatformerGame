using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{

    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (stateMachine.CurrentState != player.RollState)
            {
                Movement?.CheckIfShouldFlip(xInput);
                Movement?.SetVelocityX(playerData.movementVelocity * xInput);
            }
            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (runInput)
            {
                stateMachine.ChangeState(player.RunState);
            }
        }

    }

    private void HandleMovement()
    {

        if (xInput != Movement.FacingDirection)
        {
            stateMachine.ChangeState(player.TurnState);
            return;
        }

        if (!isExitingState)
        {
            if (stateMachine.CurrentState != player.RollState)
            {
                Movement?.CheckIfShouldFlip(xInput);
                Movement?.SetVelocityX(playerData.movementVelocity * xInput);
            }
            
        }
    }

    private void HandleIdle()
    {
        if (xInput != Movement.FacingDirection)
        {
            stateMachine.ChangeState(player.TurnState);
        }
        else
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
