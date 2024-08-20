using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBall : BaseProjectTile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        anim.SetTrigger("hit");
        rb.velocity = Vector2.zero;
    }
}
