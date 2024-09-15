using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAirAttackState : PlayerAttackState
{
    private bool isDownWardAttack = false;
    public PlayerAirAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = playerData.airAttackCounter;
    }
    protected override int AttackCounter => playerData.airAttackCounter;
    protected override AttackDetails AttackDetails => isDownWardAttack ? playerData.downWardAttackDetails : playerData.airAttackDetails[attackCounter];
    public override void Enter()
    {
        base.Enter();
        player.SetGravity(0f);
        Movement?.SetVelocityZero();
        Movement.CanSetVelocity = false;

        PlayerStats.Stamina.Decrease(playerData.airAttackStamina);
        if (isDownWardAttack)
        {
            PerformDownWardAttack();
        }

        Combat.OnBeingAttacked += Combat_OnBeingAttacked;

    }

    private void Combat_OnBeingAttacked()
    {
        stateMachine.ChangeState(player.KnockbackState);
    }

    protected override void CheckAttack()
    {
        foreach (IDamageable item in detectedDamageables.ToList())
        {
            item.Damage(AttackDetails.attackDamage);
            PlayerStats.Energy.Increase(playerData.energyGain);

        }
        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(AttackDetails.knockbackAngle, AttackDetails.knockbackStrength, Movement.FacingDirection);
        }
    }
    public override void Exit()
    {
        base.Exit();
        isDownWardAttack = false;
        player.ResetGravity();
        Movement.CanSetVelocity = true;
        player.Anim.SetBool("isDownWardAttack", false);
        player.Anim.SetBool("downWardAttackHit", false);

        Combat.OnBeingAttacked -= Combat_OnBeingAttacked;

    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isDownWardAttack && CollisionSenses.Ground)
        {
            player.Anim.SetBool("downWardAttackHit", true);
            player.ResetLayer();
        }
    }
    private void PerformDownWardAttack()
    {
        player.Anim.SetBool("isDownWardAttack", true);
    }
    public override void AnimationStartTrigger()
    {
        base.AnimationStartTrigger();
        if (isDownWardAttack)
        {
            player.SetGravity(20);
            player.ChangeLayer();
        }
    }
    public void CheckIsDownWardAttack(float yInput)
    {
        if (yInput < 0) isDownWardAttack = true;
    }

}
