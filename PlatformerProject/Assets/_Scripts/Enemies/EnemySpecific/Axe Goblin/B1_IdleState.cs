using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_IdleState : IdleState
{
    private Boss1 boss;
    public B1_IdleState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Boss1 boss) : base(etity, stateMachine, animBoolName, stateData)
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

        if (isIdleTimeOver)
        {
            if (isPlayerInMinAgroRange)
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
            else
            {
                stateMachine.ChangeState(boss.lookForPlayerState);
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
