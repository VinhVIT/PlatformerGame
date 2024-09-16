using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E6_StunState : StunState
{
    private Enemy6 enemy;

    public E6_StunState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Enemy6 enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.ChargeState);
        }
        else
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }
}
