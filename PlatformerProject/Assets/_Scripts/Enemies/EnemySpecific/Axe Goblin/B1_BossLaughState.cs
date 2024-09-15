using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_BossLaughState : BossLaughState
{
    private Boss1 boss;
    public B1_BossLaughState(Entity etity, FiniteStateMachine stateMachine, string animBoolName,D_BossLaughState stateData, Boss1 boss) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(boss.ChargeState);
        }
    }
}
