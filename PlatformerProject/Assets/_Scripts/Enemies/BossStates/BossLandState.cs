using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLandState : State
{
    protected D_BossLandState stateData;
    protected CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;
    public BossLandState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_BossLandState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (CollisionSenses.Ground)
        {
            entity.anim.SetBool("hitGround", true);

        }
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        CameraManager.Instance.ShakeWithProfile(stateData.profile, entity.ImpulseSource);

    }
}
