using System;
using System.Collections;
using System.Collections.Generic;
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
        if (spellSlotInput == 1)
        {
            currentSpellData = playerData.spell1;
        }
        else if (spellSlotInput == 2)
        {
            currentSpellData = playerData.spell2;
        }
        else if (spellSlotInput == 3)
        {
            currentSpellData = playerData.spell3;
        }
        else if (spellSlotInput == 4)
        {
            currentSpellData = playerData.spell4;
        }
        else if (spellSlotInput == 5)
        {
            currentSpellData = playerData.spell5;
        }
    }
    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(0f);

        SpellSetting.OnCastingDone += OnCastingDoneHander;
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
        SpellSetting.OnCastingDone -= OnCastingDoneHander;
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
    private void CastSpell()
    {
        switch (currentSpellData.spellType)
        {
            case SpellType.Projectile:
                CastProjectileSpell();
                break;
            case SpellType.Target:
                CastTargetSpell();
                break;
            case SpellType.Summon:
                CastSummonSpell();
                break;
            case SpellType.Area:
                CastAreaSpell();
                break;
            default:
                Debug.Log("Unknown spell type");
                break;
        }

    }
    private void CastSpellTest()
    {
        SpellHander.Instance.CastSpell(currentSpellData.spellPrefab, player.transform);
    }
    private void CastProjectileSpell()
    {
        float castRange = 1.5f;

        SpellManager.Instance.CastSpell(currentSpellData.spellPrefab, player.transform.position +
            new Vector3(castRange, 0, 0) * Movement.FacingDirection, player.transform.rotation);
    }
    private void CastTargetSpell()
    {
        SpellManager.Instance.CastSpell(currentSpellData.spellPrefab, CollisionSenses.EnemyInRange,
         player.transform.rotation);
    }
    private void CastSummonSpell()
    {
        float castRange = 1.5f;

        SpellManager.Instance.CastSpell(currentSpellData.spellPrefab, player.transform.position +
            new Vector3(castRange, 0, 0) * Movement.FacingDirection, player.transform.rotation);
    }
    private void CastAreaSpell()
    {
        float castRange = 2f;
        Vector2 castPosition = player.MovementCollider.bounds.center;
        castPosition.y -= player.MovementCollider.bounds.size.y / 2f;
        castPosition.x = player.transform.position.x + castRange * Movement.FacingDirection;

        SpellManager.Instance.CastSpell(currentSpellData.spellPrefab, castPosition, player.transform.rotation);
    }
    public bool CheckIfCanCastSpell()
    {
        return Time.time >= lastCastTime + currentSpellData.cooldownTime;
    }
}
