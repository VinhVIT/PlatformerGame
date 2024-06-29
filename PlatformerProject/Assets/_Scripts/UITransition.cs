using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        AreaTrigger.OnAreaChange += AreaTrigger_OnAreaChange;
    }

    private void AreaTrigger_OnAreaChange(Collider2D col, float delayTime)
    {
        anim.SetBool("fade", true);
        Invoke("ResetCanvasGroup", delayTime);
    }
    private void ResetCanvasGroup()
    {
        anim.SetBool("fade", false);
    }
    void OnDestroy()
    {
        AreaTrigger.OnAreaChange -= AreaTrigger_OnAreaChange;

    }
}
