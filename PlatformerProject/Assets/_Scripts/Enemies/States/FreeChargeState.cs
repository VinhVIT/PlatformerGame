using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeChargeState : ChargeState
{
    public FreeChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData)
        : base(entity, stateMachine, animBoolName, stateData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        Vector2 direction = (entity.TargetPosition - (Vector2)entity.transform.position).normalized;
        Movement?.SetVelocity(stateData.chargeSpeed, direction);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        Vector2 direction = (entity.TargetPosition - (Vector2)entity.transform.position).normalized;
        Movement?.SetVelocity(stateData.chargeSpeed, direction);

        if (CollisionSenses.Ground)// limit ground touch
        {
            Movement?.SetVelocityY(0);
        }
        if (Time.time >= startTime + stateData.chargeTime)
        {
            isChargeTimeOver = true;
        }
    }
    public override void Exit()
    {
        base.Exit();
        Movement?.SetVelocity(0, Vector2.zero);
    }
}
