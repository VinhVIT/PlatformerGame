using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected D_StunState stateData;

    private Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    public StunState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityZero();
        Movement.CanSetVelocity = false;
    }
    public override void Exit()
    {
        base.Exit();
        Movement.CanSetVelocity = true;
    }
}
