using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        AreaTrigger.OnAreaChange += AreaTrigger_OnAreaChange;
    }

    private void AreaTrigger_OnAreaChange(Collider2D d)
    {
        anim.SetBool("fade", true);
        Invoke("ResetCanvasGroup", .5f);
    }
    private void ResetCanvasGroup()
    {
        anim.SetBool("fade", false);
    }
}
