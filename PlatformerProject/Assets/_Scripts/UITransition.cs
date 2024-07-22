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
        EventManager.Trigger.OnAreaChange += AreaTrigger_OnAreaChange;
    }

    private void AreaTrigger_OnAreaChange(float delayTime, Collider2D col)
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
        EventManager.Trigger.OnAreaChange -= AreaTrigger_OnAreaChange;
    }
}
