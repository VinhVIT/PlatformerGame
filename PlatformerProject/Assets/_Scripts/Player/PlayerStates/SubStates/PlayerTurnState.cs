using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : PlayerGroundedState
{
    private float slideSpeed;

    public PlayerTurnState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        slideSpeed = playerData.turnSlideSpeed;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            Movement?.Flip();
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            Movement?.SetVelocityX(slideSpeed * Movement.FacingDirection);
            slideSpeed = Mathf.Lerp(slideSpeed, 0, playerData.turnSlideDeceleration * Time.deltaTime);
        }
    }
}
