using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellCastState : PlayerAbilityState
{
    public SpellData spellData;
    private float lastCastTime;
    private Transform spellCastRange;
    public PlayerSpellCastState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    public void SetSpellData(int spellSlotInput)
    {
        if (spellSlotInput == 1)
        {
            spellData = playerData.spell1;
        }
        else if (spellSlotInput == 2)
        {
            spellData = playerData.spell2;
        }
    }
    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(0f);

        Spell.OnCastingDone += OnCastingDoneHander;
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
        Spell.OnCastingDone -= OnCastingDoneHander;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        CastSpell();
    }
    private void CastSpell()
    {
        switch (spellData.spellType)
        {
            case SpellType.OneTime:
                CastOneHitSpell();
                break;
            case SpellType.Target:
                CastTargetSpell();
                break;
            default:
                Debug.Log("Unknown spell type");
                break;
        }

    }
    private void CastOneHitSpell()
    {
        float castRange = 1.5f;

        SpellManager.Instance.CastSpell(spellData.spellPrefab, player.transform.position +
            new Vector3(castRange, 0, 0) * Movement.FacingDirection, player.transform.rotation);
    }
    private void CastTargetSpell()
    {   
        SpellManager.Instance.CastSpell(spellData.spellPrefab, CollisionSenses.EnemyInRange,
         player.transform.rotation);
    }
    public bool CheckIfCanCastSpell()
    {
        return Time.time >= lastCastTime + spellData.cooldownTime;
    }
}
