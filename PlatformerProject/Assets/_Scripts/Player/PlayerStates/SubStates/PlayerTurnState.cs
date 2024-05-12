using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : PlayerGroundedState
{
    public PlayerTurnState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(playerData.turnFriction);


    }
    public override void Exit()
    {
        base.Exit();
        playerData.moveTimeElapsed = 0f;
        Movement.Flip();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}
