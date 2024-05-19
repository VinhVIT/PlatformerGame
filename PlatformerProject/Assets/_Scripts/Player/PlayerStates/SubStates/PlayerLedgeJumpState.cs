using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeJumpState : PlayerAbilityState
{
    public PlayerLedgeJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseJumpInput();
        player.JumpState.ResetAmountOfJumpsLeft();

        float xInput = player.InputHandler.NormInputX;
        if (xInput != 0)
        {
            float yDirection = 1.5f;
            Movement?.SetVelocity(playerData.ledgeJumpVelocity, new Vector2(xInput, yDirection).normalized);
            Movement?.CheckIfShouldFlip((int)xInput);
        }
        else
        {
            Movement?.SetVelocityY(playerData.ledgeJumpVelocity);
        }

        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);

        if (Time.time >= startTime + playerData.ledgeJumpTime)
        {
            isAbilityDone = true;
        }
    }
}
