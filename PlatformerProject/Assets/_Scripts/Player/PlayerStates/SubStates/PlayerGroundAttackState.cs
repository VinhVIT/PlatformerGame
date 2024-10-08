using System;
using System.Linq;
using UnityEngine;

public class PlayerGroundAttackState : PlayerAttackState
{
    private bool isSprintAttack = false;
    public PlayerGroundAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = playerData.groundAttackCounter;
    }
    protected override int AttackCounter => playerData.groundAttackCounter;
    protected override AttackDetails AttackDetails => isSprintAttack ? playerData.sprintAttackDetails : playerData.groundAttackDetails[attackCounter];
    public override void Enter()
    {
        base.Enter();

        if (isSprintAttack)
        {
            PerformDownWardAttack();
        }
    }
    public override void Exit()
    {
        base.Exit();
        isSprintAttack = false;
        player.Anim.SetBool("sprintAttack", false);

    }
    private void PerformDownWardAttack()
    {
        player.Anim.SetBool("sprintAttack", true);
    }
    public void CheckIsSprintAttack(bool runInput)
    {
        if (runInput) isSprintAttack = true;
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
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        SetPlayerVelocityX(0f);
    }
}
