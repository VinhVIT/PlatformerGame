using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PlayerAttackState : PlayerAbilityState
{
    protected Combat Combat => combat ?? core.GetCoreComponent(ref combat);
    private Combat combat;
    private int xInput;
    protected int attackCounter;
    private float velocityToSet;
    private float lastAttackTime;
    protected bool setVelocity;
    private bool shouldCheckFlip;

    protected List<IDamageable> detectedDamageables = new List<IDamageable>();
    protected List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

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

        SetPlayerVelocity(AttackDetails.attackMovementSpeed);
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        SetPlayerVelocity(0f);
        isAbilityDone = true;

        attackCounter++;
        lastAttackTime = Time.time;
    }
    protected void SetPlayerVelocity(float velocity)
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
            item.Damage(AttackDetails.attackDamage);

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
    public void SetFlipCheck(bool value) => shouldCheckFlip = value;
    public override void AnimationTurnOffFlipTrigger() => SetFlipCheck(false);
    public override void AnimationTurnOnFlipTrigger() => SetFlipCheck(true);
    public override void AnimationActionTrigger() => CheckAttack();
}
