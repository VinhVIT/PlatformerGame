using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraightSpell : IMoveableSpell
{
    public void Move(Rigidbody2D rb, Transform spellTransform, float speed)
    {
        rb.velocity = spellTransform.right * speed;
    }
}
