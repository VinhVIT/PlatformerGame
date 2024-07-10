using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveableSpell
{
    void Move(Rigidbody2D rb,Transform spellTransform,float speed);
}
