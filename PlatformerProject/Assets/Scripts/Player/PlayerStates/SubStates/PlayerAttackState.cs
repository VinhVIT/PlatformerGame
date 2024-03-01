using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private int xInput;
    private int attackCounter;
    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;
    private List<IDamageable> detectedDamageable = new List<IDamageable>();
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = playerData.attackCounter;
    }
    public override void Enter()
    {
        base.Enter();
        setVelocity = false;
        attackCounter++;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;

        if (shouldCheckFlip)
        {
            core.Movement.CheckIfShouldFlip(xInput);
        }

        ResetAttackCounter();
        player.Anim.SetInteger("attackCounter", attackCounter);

        if (setVelocity)
        {
            core.Movement.SetVelocityX(velocityToSet * core.Movement.FacingDirection);
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
    }
    private void SetPlayerVelocity(float velocity)
    {
        core.Movement.SetVelocityX(velocity * core.Movement.FacingDirection);
        velocityToSet = velocity;
        setVelocity = true;
    }
    public void AddToDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            detectedDamageable.Add(damageable);
        }
    }
    public void RemoveFromDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            detectedDamageable.Remove(damageable);
        }
    }
    private void CheckAttack()
    {
        foreach (IDamageable item in detectedDamageable)
        {
            item.Damage(playerData.attackDamage[attackCounter]);
        }
    }
    public void SetFlipCheck(bool value) => shouldCheckFlip = value;
    public void AnimationTurnOffFlipTrigger() => SetFlipCheck(false);
    public void AnimationTurnOnFlipTrigger() => SetFlipCheck(true);
    public void AnimationActionTrigger()
    {
        CheckAttack();
    }
}
