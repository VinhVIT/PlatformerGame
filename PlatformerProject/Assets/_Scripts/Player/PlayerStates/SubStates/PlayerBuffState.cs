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
        ApplySwordBuff();
    }
    private void ShieldBuff()
    {
        player.Anim.SetInteger("buffSlot", buffSlotInput);
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (buffSlotInput == 1)
        {
            player.StartCoroutine(ApplySwordBuff());
        }
        else if (buffSlotInput == 2)
        {
            player.StartCoroutine(ApplyShieldBuff());
        }
        stateMachine.ChangeState(player.IdleState);
    }
    private IEnumerator ApplySwordBuff()
    {   
        int amount = 15;
        Debug.Log("ApplySwordBuff");
        player.AirAttackState.IncreaseAttackBonus(amount);
        player.GroundAttackState.IncreaseAttackBonus(amount);

        yield return new WaitForSeconds(10f);
        Debug.Log("SwordBuffEnd");

        player.AirAttackState.DecreaseAttackBonus(amount);
        player.GroundAttackState.DecreaseAttackBonus(amount);
    }
    private IEnumerator ApplyShieldBuff()
    {   
        int amount = 15;
        Debug.Log("ApplyShieldBuff");
        player.BlockState.SetBlockStaminaToZero();
        player.BlockCounterState.IncreaseAttackBonus(amount);

        yield return new WaitForSeconds(10f);

        Debug.Log("ShieldBuffEnd");
        player.BlockState.ResetBlockStamina();
        player.BlockCounterState.DecreaseAttackBonus(amount);
    }
}


