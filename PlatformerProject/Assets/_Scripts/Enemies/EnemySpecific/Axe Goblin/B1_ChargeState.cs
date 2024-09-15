using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_ChargeState : ChargeState
{
    private Boss1 boss;

    private float shakeTimer = 0f;
    public B1_ChargeState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Boss1 boss) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
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

    private void ShakePerTime(float timer)
    {
        shakeTimer += Time.deltaTime;
        if (shakeTimer >= timer)
        {
            CameraManager.instance.ShakeWithProfile(stateData.profile, boss.ImpulseSource);
            shakeTimer = 0f;
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        ShakePerTime(stateData.shakeTime);
        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(boss.MeleeAttackState);
        }
        else if (!isDectectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(boss.lookForPlayerState);
        }
        else if (isChargeTimeOver)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(boss.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(boss.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
