using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    public event Action OnBeingAttacked;

    private Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;
    private Stats Stats => stats ?? core.GetCoreComponent(ref stats);
    private Stats stats;
    private ParticleManager ParticleManager => particleManager ?? core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;

    [SerializeField] private GameObject damageParticle;
    [SerializeField] private float maxKnockbackTime = 0.2f;
    private bool isKnockbackActive;
    private bool canDamage = true;
    private float knockbackStartTime;

    public override void LogicUpdate()
    {
        CheckKnockback();
    }
    public void Damage(int amount)
    {
        if (canDamage)
        {
            Stats?.DecreaseHealth(amount);
        }
        OnBeingAttacked?.Invoke();
        // ParticleManager?.StartParicleWithRandomRotation(damageParticle);
    }
    public void Knockback(Vector2 angle, float strength, int direction)
    {   
        Movement?.SetVelocity(strength, angle, direction);
        Movement.CanSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.unscaledTime;
    }
    private void CheckKnockback()
    {
        if (isKnockbackActive && ((Movement.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground) || Time.unscaledTime >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
        }
    }
    public void SetCanDamage(bool value)
    {
        canDamage = value;
    }
}
