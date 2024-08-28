using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E6_PlayerDetectedState : PlayerDetectedState
{
    private Enemy6 enemy;

    public E6_PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Enemy6 enemy) : base(etity, stateMachine, animBoolName, stateData)
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
        else if (!isPlayerInMaxAgroRange)
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
