using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_BossDashState : BossDashState
{
    private Boss1 boss;
    public B1_BossDashState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_BossDashState stateData, Boss1 boss) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(boss.MeleeAttackState);
        }
        else if (!isDectectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(boss.MeleeAttackState);
        }
    }
}
