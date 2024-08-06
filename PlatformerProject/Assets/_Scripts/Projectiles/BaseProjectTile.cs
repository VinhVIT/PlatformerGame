using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseProjectTile : MonoBehaviour
{
    [SerializeField] private AttackDetails attackDetails;
    private List<IDamageable> detectedDamageables = new List<IDamageable>();
    private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();
    void OnTriggerEnter2D(Collider2D collision)
    {
        AddToDetected(collision);
        CheckAttack();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        RemoveFromDetected(collision);
    }
    private void AddToDetected(Collider2D collision)
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
    private void RemoveFromDetected(Collider2D collision)
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
            item.Damage(attackDetails.attackDamage);

        }
        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(attackDetails.knockbackAngle, attackDetails.knockbackStrength, (int)transform.right.x);
        }
    }
}
