using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private int xInput;
    private int attackCounter;
    private float velocityToSet;
    private float lastAttackTime;
    private bool setVelocity;
    private bool shouldCheckFlip;

    private List<IDamageable> detectedDamageables = new List<IDamageable>();
    private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = playerData.attackCounter;
    }
    public override void Enter()
    {
        base.Enter();
        setVelocity = false;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;

        if (shouldCheckFlip)
        {
            Movement?.CheckIfShouldFlip(xInput);
        }

        ResetAttackCounter();
        player.Anim.SetInteger("attackCounter", attackCounter);

        if (setVelocity)
        {
            Movement?.SetVelocityX(velocityToSet * Movement.FacingDirection);
        }
    }
    private void ResetAttackCounter()
    {
        if (attackCounter >= playerData.attackCounter)
        {
            attackCounter = 0;
        }
    }
    public override void AnimationStartTrigger()
    {
        base.AnimationStartTrigger();

        SetPlayerVelocity(playerData.attackMovementSpeed[attackCounter]);
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        SetPlayerVelocity(0f);
        isAbilityDone = true;

        attackCounter++;
        lastAttackTime = Time.time;
    }
    private void SetPlayerVelocity(float velocity)
    {
        Movement?.SetVelocityX(velocity * Movement.FacingDirection);
        velocityToSet = velocity;
        setVelocity = true;
    }
    public void AddToDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            detectedDamageables.Add(damageable);
        }

        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();
        if (knockbackable != null)
        {
            detectedKnockbackables.Add(knockbackable);
        }
    }
    public void RemoveFromDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            detectedDamageables.Remove(damageable);
        }

        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();
        if (knockbackable != null)
        {
            detectedKnockbackables.Remove(knockbackable);
        }
    }
    private void CheckAttack()
    {
        foreach (IDamageable item in detectedDamageables.ToList())
        {
            item.Damage(playerData.attackDamage[attackCounter]);
        }
        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(playerData.knockbackAngle[attackCounter], playerData.knockbackStrength[attackCounter], Movement.FacingDirection);
        }
    }
    public void CheckToResetAttackCounter()
    {
        if (Time.time >= lastAttackTime + playerData.attackResetCooldown)
        {
            attackCounter = 0;
        }
    }
    public void SetFlipCheck(bool value) => shouldCheckFlip = value;
    public void AnimationTurnOffFlipTrigger() => SetFlipCheck(false);
    public void AnimationTurnOnFlipTrigger() => SetFlipCheck(true);
    public void AnimationActionTrigger() => CheckAttack();
}
