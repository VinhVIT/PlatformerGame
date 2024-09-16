using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_StunState : StunState
{
    private Boss1 boss;

    public B1_StunState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Boss1 boss) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (boss.IsEngaged)
        {
            stateMachine.ChangeState(boss.BossDashState);
        }
        else
        {
            stateMachine.ChangeState(boss.ChargeState);
        }
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        CameraManager.Instance.ShakeWithProfile(stateData.profile, boss.ImpulseSource);
    }

}
