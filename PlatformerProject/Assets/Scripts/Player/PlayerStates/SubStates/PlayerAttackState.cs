using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private int xInput;
    private int attackCounter;
    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = playerData.attackCounter;
    }
    public override void Enter()
    {
        base.Enter();
        setVelocity = false;
        attackCounter++;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;

        if (shouldCheckFlip)
        {
            player.CheckIfShouldFlip(xInput);
        }

        ResetAttackCounter();
        player.Anim.SetInteger("attackCounter", attackCounter);

        if (setVelocity)
        {
            player.SetVelocityX(velocityToSet * player.FacingDirection);
        }
    }
    private void ResetAttackCounter()
    {
        if (attackCounter >= playerData.attackCounter)
        {
            attackCounter = 0;
        }
    }
    public override void AnimationStartTrigger()
    {
        base.AnimationStartTrigger();
        SetPlayerVelocity(playerData.attackMovementSpeed[attackCounter]);

    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        SetPlayerVelocity(0f);
        isAbilityDone = true;
    }
    private void SetPlayerVelocity(float velocity)
    {
        player.SetVelocityX(velocity * player.FacingDirection);
        velocityToSet = velocity;
        setVelocity = true;
    }
    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }
    public void AnimationTurnOffFlipTrigger()
    {
        SetFlipCheck(false);
    }
    public void AnimationTurnOnFlipTrigger()
    {
        SetFlipCheck(true);
    }
}
