using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PlayerAttackState : PlayerAbilityState
{   
    public event Action OnAttackBonusChanged;
    private int xInput;
    protected int attackCounter;
    private float velocityToSet;
    private float lastAttackTime;
    protected bool setVelocity;
    private bool shouldCheckFlip;
    public int EnergyGain { get; private set; }
    public int AttackBonus { get; private set; }
    protected List<IDamageable> detectedDamageables = new List<IDamageable>();
    protected List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        EnergyGain = playerData.energyGain;
        AttackBonus = 0;
    }
    protected abstract int AttackCounter { get; }
    protected abstract AttackDetails AttackDetails { get; }

    public override void Enter()
    {
        base.Enter();
        setVelocity = false;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;

        ResetAttackCounter();

        if (shouldCheckFlip)
        {
            Movement?.CheckIfShouldFlip(xInput);
        }

        player.Anim.SetInteger("attackCounter", attackCounter);

        if (setVelocity)
        {
            Movement?.SetVelocityX(velocityToSet * Movement.FacingDirection);
        }
    }
    public override void AnimationStartTrigger()
    {
        base.AnimationStartTrigger();

        SetPlayerVelocityX(AttackDetails.attackMovementSpeed);
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        SetPlayerVelocityX(0f);
        isAbilityDone = true;

        attackCounter++;
        lastAttackTime = Time.time;
    }
    protected void SetPlayerVelocityX(float velocity)
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
    protected virtual void CheckAttack()
    {

        foreach (IDamageable item in detectedDamageables.ToList())
        {
            item.Damage(AttackDetails.attackDamage + AttackBonus);

        }
        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(AttackDetails.knockbackAngle, AttackDetails.knockbackStrength, Movement.FacingDirection);
        }
    }
    private void ResetAttackCounter()
    {
        if (attackCounter >= AttackCounter)
        {
            attackCounter = 0;
        }
    }
    public void CheckToResetAttackCounter()
    {
        if (Time.time >= lastAttackTime + playerData.attackResetCooldown)
        {
            attackCounter = 0;
        }
    }
    public void IncreaseEneryGain(int amount) => EnergyGain += amount;
    public void IncreaseAttackBonus(int amount)
    {
        AttackBonus += amount;
        OnAttackBonusChanged?.Invoke();
    }

    public void DecreaseAttackBonus(int amount)
    {
        AttackBonus -= amount;
        OnAttackBonusChanged?.Invoke();
    }

    public void SetFlipCheck(bool value) => shouldCheckFlip = value;
    public override void AnimationTurnOffFlipTrigger() => SetFlipCheck(false);
    public override void AnimationTurnOnFlipTrigger() => SetFlipCheck(true);
    public override void AnimationActionTrigger() => CheckAttack();
}
