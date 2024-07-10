using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastable
{
    Vector3 GetCastPosition(Transform caster);
}
