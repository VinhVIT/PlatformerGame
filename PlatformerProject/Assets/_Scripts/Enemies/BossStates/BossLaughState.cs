using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaughState : State
{
    protected D_BossLaughState stateData;

    public BossLaughState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_BossLaughState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        CameraManager.Instance.ShakeWithProfile(stateData.profile, entity.ImpulseSource);

    }
}
