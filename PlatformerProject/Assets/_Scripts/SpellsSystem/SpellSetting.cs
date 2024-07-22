using System;
using UnityEngine;

public class SpellSetting : MonoBehaviour
{
    [SerializeField] private SpellData data;
    private Animator anim;
    private Rigidbody2D rb;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }
    public Vector3 GetCastPosition(Transform caster)
    {
        ICastable castable = null;

        switch (data.castType)
        {
            case CastType.Infront:
                castable = new CastInFront();
                break;
            case CastType.OnGround:
                castable = new CastOnGround();
                break;
            case CastType.EnemyPos:
                castable = new CastAtEnemyPos();
                break;
        }

        if (castable != null)
        {
            return castable.GetCastPosition(caster);
        }
        else
        {
            Debug.LogError("No valid cast position type selected!");
            return Vector3.zero;
        }
    }
    public void CastTimeFinishTrigger()
    {
        EventManager.Player.OnSpellCastDone?.Invoke();
        if (data.moveAble)
        {
            IMoveableSpell moveableSpell = new MoveStraightSpell();
            moveableSpell.Move(rb, transform, data.moveSpeed);
        }
    }
    public void AnimationFinishTrigger()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(data.damage);
        }
        
        if (data.damageType == DamageType.Single)
        {
            anim.SetTrigger("hit");
            rb.velocity = Vector2.zero;

        }
    }
}

