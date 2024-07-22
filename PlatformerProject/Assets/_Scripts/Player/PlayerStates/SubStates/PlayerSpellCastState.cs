using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerSpellCastState : PlayerAbilityState
{
    public SpellData currentSpellData;
    private float lastCastTime;
    public PlayerSpellCastState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    public void SetSpellData(int spellSlotInput)
    {
        if (spellSlotInput < 1 || spellSlotInput > playerData.spellDatas.Length) return;
        currentSpellData = playerData.spellDatas[spellSlotInput - 1];
    }
    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(0f);

        EventManager.Player.OnSpellCastDone += OnCastingDoneHander;
    }

    private void OnCastingDoneHander()
    {
        isAbilityDone = true;
        stateMachine.ChangeState(player.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
        lastCastTime = Time.time;
        EventManager.Player.OnSpellCastDone -= OnCastingDoneHander;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        CastSpellTest();
    }
    private void CastSpellTest()
    {
        SpellHander.Instance.CastSpell(currentSpellData.spellPrefab, player.transform);
    }

    public bool CheckIfCanCastSpell()
    {
        return Time.time >= lastCastTime + currentSpellData.cooldownTime;
    }
}
