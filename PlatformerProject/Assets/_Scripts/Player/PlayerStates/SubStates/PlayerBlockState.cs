using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerGroundedState
{
    private bool isPerfectBlock;
    public PlayerBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        isPerfectBlock = true;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityZero();

        if (Time.time >= startTime + playerData.perfectBlockTime)
        {   
            isPerfectBlock = false;
        }
        Debug.Log(isPerfectBlock);
        if (!blockInput)
        {   
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
