using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_PlayerDetectedState : PlayerDetectedState
{
    private Boss1 boss;

    public B1_PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Boss1 boss) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
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
            stateMachine.ChangeState(boss.MeleeAttackState);
        }
        else if (performLongRangeAction)
        {    
            if (boss.IsEngaged)
            {
                stateMachine.ChangeState(boss.BossDashState);
            }
            else
            {
                stateMachine.ChangeState(boss.ChargeState);
            }
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(boss.lookForPlayerState);
        }
        // else if (!isDetectingLedge)
        // {
        //     Movement?.Flip();
        //     stateMachine.ChangeState(boss.moveState);
        // }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
