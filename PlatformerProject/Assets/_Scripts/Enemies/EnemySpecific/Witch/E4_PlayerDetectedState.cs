using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class E4_PlayerDetectedState : PlayerDetectedState
{
    private Enemy4 enemy;

    public E4_PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Enemy4 enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
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
            if(Time.time >= enemy.DodgeState.startTime + enemy.dodgeStateData.dodgeCooldown)
            {
                stateMachine.ChangeState(enemy.DodgeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.rangedAttackState);
            }            
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.rangedAttackState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
