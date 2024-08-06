using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffState : PlayerGroundedState
{
    public PlayerBuffState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        DetermineBuff();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityZero();
    }
    private void DetermineBuff()
    {
        if (buffSlotInput == 1)
        {
            SwordBuff();
        }
        else if (buffSlotInput == 2)
        {
            ShieldBuff();
        }
    }
    private void SwordBuff()
    {
        player.Anim.SetInteger("buffSlot", buffSlotInput);
    }
    private void ShieldBuff()
    {
        player.Anim.SetInteger("buffSlot", buffSlotInput);
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(player.IdleState);
    }
}


