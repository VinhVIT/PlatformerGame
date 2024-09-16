using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_StunState : StunState
{
    private Enemy4 enemy;

    public E4_StunState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Enemy4 enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.rangedAttackState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }
}
