using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_BossLandState : BossLandState
{   
    private Boss1 boss;
    public B1_BossLandState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_BossLandState stateData, Boss1 boss) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }
    public override void Enter()
    {   
        base.Enter();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(isAnimationFinished)
        {
            stateMachine.ChangeState(boss.BossLaughState);
        }
    }
}
