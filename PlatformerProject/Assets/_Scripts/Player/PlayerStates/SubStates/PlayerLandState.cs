using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    private bool canMove = false;
    private bool sprintJump;
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        DetermineLandType();
    }
    private void DetermineLandType()
    {
        sprintJump = player.Anim.GetBool("sprintJump");
        if (sprintJump)//sprintLand
        {
            Movement?.SetVelocityX(playerData.sprintSlideVelocity * Movement.FacingDirection);
        }
        else//normalLand
        {
            Movement?.SetVelocityZero();
        }
    }
    public override void Exit()
    {
        base.Exit();
        canMove = false;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (!isExitingState)
        {

            if (xInput != 0 && canMove)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            // else
            // {
            //     stateMachine.ChangeState(player.IdleState);
            // }
        }
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        Movement?.SetVelocityZero();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        canMove = true;
        player.Anim.SetBool("sprintJump", false);
        stateMachine.ChangeState(player.IdleState);
    }
}