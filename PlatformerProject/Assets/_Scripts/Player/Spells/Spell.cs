using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public static event Action OnCastingDone;
    [SerializeField] private SpellData data;

    protected Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void CastTimeFinishTrigger()
    {
        OnCastingDone?.Invoke();
    }
    protected virtual void AnimationFinishTrigger()
    {
        Destroy(gameObject);
    }
}
