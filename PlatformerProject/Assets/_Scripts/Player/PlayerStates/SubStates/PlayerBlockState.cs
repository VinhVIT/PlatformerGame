using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerGroundedState
{
    public bool CanBlock { get; private set; }
    private bool isPerfectBlock;
    private float lastBlockTime;
    public PlayerBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();

        CanBlock = false;
        Combat.SetCanDamage(false);
        isPerfectBlock = true;

        Combat.OnBeingAttacked += Combat_OnBeingAttacked;
    }

    private void Combat_OnBeingAttacked()
    {
        if (isPerfectBlock)
        {
            stateMachine.ChangeState(player.BlockCounterState);
        }
        else
        {
            player.Anim.SetTrigger("gotHit");
            player.StartCoroutine(BlockingForce());

        }
    }
    private IEnumerator BlockingForce()
    {
        Movement?.SetVelocityX(playerData.blockKnockbackForce * Movement.FacingDirection);
        yield return new WaitForSeconds(playerData.blockKnockbackTime);
        Movement?.SetVelocityZero();
    }
    public override void Exit()
    {
        base.Exit();
        // Movement.CanSetVelocity = true;
        Combat.SetCanDamage(true);

        Combat.OnBeingAttacked -= Combat_OnBeingAttacked;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // Movement.CanSetVelocity = false;

        if (Time.time >= startTime + playerData.perfectBlockTime)
        {
            //perfectBlock time pass
            isPerfectBlock = false;
            // player.Anim.SetBool("blockCounter", false);

        }
        if (!PlayerStats.Stamina.EnoughToUse(playerData.blockStamina))
        {   //Cant block anymore
            player.Anim.SetBool("blockFailed", true);
        }
        if (!blockInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public bool CheckIfCanBlock()
    {
        return CanBlock && Time.time >= lastBlockTime + playerData.blockRecoveryTime;
    }
    public void ResetCanBlock() => CanBlock = true;

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
        PlayerStats.Stamina.Decrease(30);
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Anim.SetBool("blockFailed", false);
        lastBlockTime = Time.time;
        stateMachine.ChangeState(player.IdleState);
    }
}
