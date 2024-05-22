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

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            if (stateMachine.CurrentState != player.SlideState)
            {
                Movement?.CheckIfShouldFlip(xInput);
                Movement?.SetVelocityX(playerData.movementVelocity * xInput);
            }
            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}