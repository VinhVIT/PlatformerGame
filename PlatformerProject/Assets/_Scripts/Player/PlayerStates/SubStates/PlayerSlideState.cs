using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerAbilityState
{
    public bool CanSlide { get; private set; }
    private Vector2 slideDirection;
    private float lastSlideTime;
    private Vector2 lastAIPos;

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

        // PlaceAfterImage();

        if (Time.unscaledTime >= startTime + playerData.slideTime && !isTouchingCeiling)
        {
            // CheckIfShouldPlaceAfterImage();

            isAbilityDone = true;
            Time.timeScale = 1f;

        }

        if (!CollisionSenses.Ground)
        {
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Movement?.SetVelocityZero();
        lastSlideTime = Time.time;
        Time.timeScale = 1f;
    }
    public bool CheckIfCanSlide()
    {
        return CanSlide && Time.time >= lastSlideTime + playerData.slideCooldown;
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if (Vector2.Distance(player.transform.position, lastAIPos) >= playerData.distBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        lastAIPos = player.transform.position;
    }
    public void ResetCanSlide() => CanSlide = true;
}
