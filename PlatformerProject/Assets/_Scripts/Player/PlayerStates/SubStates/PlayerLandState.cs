using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    private bool canMove = false;
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();
        // ParticleManager.StartParticle(playerData.dustFallParticle,CollisionSenses.GroundCheck.position,Quaternion.identity);
        //TODO: Fix Particle bug when destroy

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
            // // {
            //     stateMachine.ChangeState(player.IdleState);
            // // }
        }
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        canMove = true;
    }
}