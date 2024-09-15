using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_MeleeAttackState : MeleeAttackState
{
    private Boss1 boss;

    public B1_MeleeAttackState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Boss1 boss) : base(etity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        if (boss.IsEngaged)
        {
            entity.anim.SetBool("heavyAttack", true);
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(boss.idleState);
            // if (isPlayerInMinAgroRange)
            // {
            //     stateMachine.ChangeState(boss.ChargeState);
            // }
            // else
            // {
            //     stateMachine.ChangeState(boss.lookForPlayerState);
            // }
        }
    }
    public override void TriggerAttack()
    {
        base.TriggerAttack();
        if (boss.IsEngaged)
        {
            CameraManager.instance.ShakeWithProfile(stateData.profile[1], entity.ImpulseSource);

        }
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        CameraManager.instance.ShakeWithProfile(stateData.profile[0], entity.ImpulseSource);

    }
}
