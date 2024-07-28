using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerGroundedState
{
    public bool CanBlock { get; private set; }
    private bool isPerfectBlock;
    private int blockCount;
    private float lastBlockTime;
    public PlayerBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        CanBlock = false;
        blockCount = 0;
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
        }
    }

    public override void Exit()
    {
        base.Exit();
        Movement.CanSetVelocity = true;
        Combat.SetCanDamage(true);

        Combat.OnBeingAttacked -= Combat_OnBeingAttacked;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityZero();
        Movement.CanSetVelocity = false;

        if (Time.time >= startTime + playerData.perfectBlockTime)
        {
            //perfectBlock time pass
            isPerfectBlock = false;
            // player.Anim.SetBool("blockCounter", false);

        }
        if (blockCount >= 3)
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
        blockCount++;
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Anim.SetBool("blockFailed", false);
        lastBlockTime = Time.time;
        stateMachine.ChangeState(player.IdleState);
    }
}
