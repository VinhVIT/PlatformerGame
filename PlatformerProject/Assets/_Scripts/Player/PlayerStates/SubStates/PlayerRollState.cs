using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerAbilityState
{
    public bool CanRoll { get; private set; }
    private Vector2 rollDirection;
    private float lastRollTime;
    private Vector2 lastAIPos;

    public PlayerRollState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        CanRoll = false;
        rollDirection = Vector2.right * Movement.FacingDirection;
        
        Time.timeScale = .75f;
        startTime = Time.unscaledTime;
        Movement?.SetVelocity(playerData.rollVelocity, rollDirection);

        player.ChangeLayer();

        PlayerStats.Stamina.Decrease(playerData.slideStamina);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        PlaceAfterImage();

        if (Time.unscaledTime >= startTime + playerData.rollTime && !isTouchingCeiling)
        {
            CheckIfShouldPlaceAfterImage();

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
        player.ResetLayer();
        lastRollTime = Time.time;
        Time.timeScale = 1f;
    }
    public bool CheckIfCanRoll()
    {
        return CanRoll && Time.time >= lastRollTime + playerData.rollCooldown;
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
    public void ResetCanRoll() => CanRoll = true;
}
