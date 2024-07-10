using UnityEngine;

public class CastOnGround : ICastable
{
    public Vector3 GetCastPosition(Transform caster)
    {
        float castRange = 1.5f;
        Vector3 castDirection = caster.right;
        BoxCollider2D casterCol = caster.GetComponent<BoxCollider2D>();
        Vector2 castPosition = casterCol.bounds.center;
        castPosition.y -= casterCol.bounds.size.y / 2f;
        castPosition.x = caster.transform.position.x + castDirection.x * castRange;
        return castPosition;
    }
}
