using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_StunState : StunState
{
    private Enemy1 enemy;

    public E1_StunState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Enemy1 enemy) : base(etity, stateMachine, animBoolName, stateData)
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
