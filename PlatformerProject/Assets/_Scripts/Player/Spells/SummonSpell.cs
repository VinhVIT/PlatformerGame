using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSpell : Spell
{
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    protected override void CastTimeFinishTrigger()
    {
        base.CastTimeFinishTrigger();
    }
    protected override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
    void OnTriggerEnter2D(Collider2D other)
    {   
        anim.SetTrigger("hit");
    }
}
