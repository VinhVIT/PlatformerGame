using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{   
    private float movingTime;
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        movingTime = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput != 0)
        {
            HandleMovement();
        }
        else
        {
            HandleIdle();
        }
    }

    private void HandleMovement()
    {
        movingTime += Time.deltaTime;

        if (movingTime > playerData.maxMovingTime && xInput != Movement.FacingDirection)
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
        if (movingTime > playerData.maxMovingTime && xInput != Movement.FacingDirection)
        {
            stateMachine.ChangeState(player.TurnState);
        }
        else
        {
            stateMachine.ChangeState(player.IdleState);
            movingTime = 0;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
