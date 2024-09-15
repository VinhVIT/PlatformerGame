using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEngageState : State
{
    protected D_BossEngageState stateData;
    private Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    private SpriteRenderer sr;
    private bool startEngage;
    public BossEngageState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_BossEngageState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();
        sr = entity.GetComponent<SpriteRenderer>();

        startEngage = false;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityZero();

        if (startEngage)
        {
            sr.material = stateData.engageMaterial;
            entity.transform.localScale = Vector3.MoveTowards(entity.transform.localScale, stateData.engageSize, Time.deltaTime);
        }

    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        startEngage = true;
    }
}
