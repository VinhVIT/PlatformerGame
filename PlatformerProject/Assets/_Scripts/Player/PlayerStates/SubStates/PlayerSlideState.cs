using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerAbilityState
{
    public bool CanSlide { get; private set; }
    private Vector2 slideDirection;
    private float lastSlideTime;
    private bool jumpInput;
    public PlayerSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        CanSlide = false;
        player.InputHandler.UseSlideInput();
        slideDirection = Vector2.right * Movement.FacingDirection;

        Time.timeScale = .75f;
        startTime = Time.unscaledTime;
        Movement?.SetVelocity(playerData.slideVelocity, slideDirection);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        jumpInput = player.InputHandler.JumpInput;

        if (Time.unscaledTime >= startTime + playerData.slideTime)
        {
            isAbilityDone = true;
            Time.timeScale = 1f;

            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
        }

        if (!CollisionSenses.Ground)
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if (jumpInput)
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Movement?.SetVelocityZero();
        lastSlideTime = Time.time;
    }
    public bool CheckIfCanSlide()
    {
        return CanSlide && Time.time >= lastSlideTime + playerData.slideCooldown;
    }

    public void ResetCanSlide() => CanSlide = true;
}
