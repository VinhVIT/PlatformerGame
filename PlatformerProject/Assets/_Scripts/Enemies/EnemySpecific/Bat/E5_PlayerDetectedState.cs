using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_PlayerDetectedState : PlayerDetectedState
{
    private Enemy5 enemy;

    public E5_PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Enemy5 enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        else if (performLongRangeAction)
        {            
            stateMachine.ChangeState(enemy.ChargeState);
        }
        else if (!isPlayerInDetectionRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        // else if (!isDetectingLedge)
        // {
        //     Movement?.Flip();
        //     stateMachine.ChangeState(enemy.MoveState);
        // }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
