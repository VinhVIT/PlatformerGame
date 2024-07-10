using UnityEngine;

public class CastAtEnemyPos : ICastable
{
    public Vector3 GetCastPosition(Transform caster)
    {
        RaycastHit2D hit = Physics2D.BoxCast(caster.position, new Vector2(0.5f, 3f), 0, caster.right, 5, LayerMask.GetMask("Damageable"));
        if (hit.collider != null)
        {
            Vector2 enemyCenterBottomPosition = hit.collider.bounds.center;
            enemyCenterBottomPosition.y -= hit.collider.bounds.size.y / 2f;

            return enemyCenterBottomPosition;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
