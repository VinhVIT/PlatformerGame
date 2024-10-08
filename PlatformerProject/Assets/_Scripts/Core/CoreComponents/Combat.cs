using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    public event Action OnBeingAttacked;
    public event Action<int> OnGotDamaged;
    private Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;
    private Stats Stats => stats ?? core.GetCoreComponent(ref stats);
    private Stats stats;
    private ParticleManager ParticleManager => particleManager ?? core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;

    [SerializeField] private GameObject damageParticle;
    [SerializeField] private bool canBeKnockback = true;
    [SerializeField] private float maxKnockbackTime = 0.2f;
    private bool isKnockbackActive;
    private bool canDamage = true;
    private float knockbackStartTime;
    [Space]
    [Header("Damage Flash")]
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = .25f;
    [SerializeField] private AnimationCurve flashSpeedCurve;
    private SpriteRenderer sr;
    private Material material;
    protected override void Awake()
    {
        base.Awake();
        sr = transform.parent.parent.GetComponent<SpriteRenderer>();
        material = sr.material;
    }
    public override void LogicUpdate()
    {
        CheckKnockback();
    }
    public void Damage(int amount)
    {
        if (canDamage)
        {
            Stats?.Health.Decrease(amount);
            Debug.Log(amount);
            OnGotDamaged?.Invoke(amount);
            ParticleManager?.StartParicleWithRandomRotation(damageParticle);
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(DamageFlasher());
            }
        }
        OnBeingAttacked?.Invoke();
    }
    public void Knockback(Vector2 angle, float strength, int direction)
    {
        if (canBeKnockback)
        {
            Movement?.SetVelocity(strength, angle, direction);
            Movement.CanSetVelocity = false;
            isKnockbackActive = true;
            knockbackStartTime = Time.unscaledTime;
        }
    }
    private void CheckKnockback()
    {
        if (isKnockbackActive && ((Movement.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground) || Time.unscaledTime >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
        }
    }
    public void ResetKnockbackState()
    {
        isKnockbackActive = false;
        Movement.CanSetVelocity = true;
    }
    public void SetCanDamage(bool value)
    {
        canDamage = value;
    }
    private IEnumerator DamageFlasher()
    {
        material.SetColor("_FlashColor", flashColor);
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentFlashAmount = Mathf.Lerp(1f, flashSpeedCurve.Evaluate(elapsedTime), (elapsedTime / flashDuration));
            material.SetFloat("_FlashAmount", currentFlashAmount);

            yield return null;
        }
    }
}
