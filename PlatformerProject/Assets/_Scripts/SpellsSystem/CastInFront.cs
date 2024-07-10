using UnityEngine;

public class CastInFront : ICastable
{
    public Vector3 GetCastPosition(Transform caster)
    {
        float castRange = 1.5f;
        Vector3 castDirection = caster.right;
        return caster.position + castDirection * castRange;
    }
}
