using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProjectileSpell : Spell
{
    [SerializeField]private float moveSpeed = 10f;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    protected override void CastTimeFinishTrigger()
    {
        base.CastTimeFinishTrigger();
        rb.velocity = transform.right * moveSpeed;

    }
    protected override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        anim.SetTrigger("hit");
        rb.velocity = Vector2.zero;
    }
}
