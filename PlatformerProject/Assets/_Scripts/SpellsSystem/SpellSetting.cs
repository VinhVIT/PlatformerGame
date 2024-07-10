using System;
using UnityEngine;

public class SpellSetting : MonoBehaviour
{
    public static event Action OnCastingDone;
    [SerializeField] private CastPosition castPositionType;
    [SerializeField] private bool moveAble;
    [ConditionalHide("moveAble", true)]
    [SerializeField] private float moveSpeed = 10f;
    private Animator anim;
    private Rigidbody2D rb;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public Vector3 GetCastPosition(Transform caster)
    {
        ICastable castable = null;

        switch (castPositionType)
        {
            case CastPosition.Infront:
                castable = new CastInFront();
                break;
            case CastPosition.OnGround:
                castable = new CastOnGround();
                break;
            case CastPosition.EnemyPos:
                castable = new CastAtEnemyPos();
                break;
        }

        if (castable != null)
        {
            return castable.GetCastPosition(caster);
        }
        else
        {
            Debug.LogError("No valid cast position type selected!");
            return Vector3.zero;
        }
    }
    public void CastTimeFinishTrigger()
    {
        OnCastingDone?.Invoke();
        if (moveAble)
        {
            IMoveableSpell moveableSpell = new MoveStraightSpell();
            moveableSpell.Move(rb, transform, moveSpeed);
        }
        // rb.velocity = transform.right * moveSpeed;
    }
    public void AnimationFinishTrigger()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        anim.SetTrigger("hit");
        rb.velocity = Vector2.zero;
    }
}

public enum CastPosition
{
    Infront,
    OnGround,
    EnemyPos
}
