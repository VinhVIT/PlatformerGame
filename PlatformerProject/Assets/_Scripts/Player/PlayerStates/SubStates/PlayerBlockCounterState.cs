using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBlockCounterState : PlayerAttackState
{
    public PlayerBlockCounterState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = 0;
    }
    protected override int AttackCounter => 0;

    protected override AttackDetails AttackDetails => playerData.blockCounterAttackDetails[attackCounter];
    public override void DoChecks()
    {
        base.DoChecks();
    }
    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0;
        ParticleManager?.StartParticle(playerData.blockFX);

    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityZero();
    }
    protected override void CheckAttack()
    {   
        bool counterSuccess = false;
        foreach (IDamageable item in detectedDamageables.ToList())
        {
            item.Damage(AttackDetails.attackDamage);
            counterSuccess = true;
        }
        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(AttackDetails.knockbackAngle, AttackDetails.knockbackStrength, Movement.FacingDirection);
        }

        if (counterSuccess)
        {
            EventManager.Player.OnCounterSuccess?.Invoke();
        }
    }
    public override void AnimationTrigger()
    {
        Time.timeScale = 1f;//reset Timescale before knockback
        base.AnimationTrigger();
    }
}
