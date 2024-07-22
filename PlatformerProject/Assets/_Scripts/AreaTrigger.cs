using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AreaTrigger : MonoBehaviour
{
    private Transform destination;
    private bool isChangeArea = false;
    private float delayTime = 0.5f;// THIS VALUE DECIDE HOW LONG THE CHANGE START
    [SerializeField] private Collider2D nextAreaBound;

    private void Start()
    {
        destination = transform.GetChild(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // move to next area
            EventManager.Trigger.OnAreaChange?.Invoke(delayTime, nextAreaBound);
            StartCoroutine(FadeOutThenChangePosition(other, destination));
        }
    }
    private IEnumerator FadeOutThenChangePosition(Collider2D other, Transform pos)
    {
        PlayerInputHandler.DeactivatePlayerControl();
        SceneFadeManager.Instance.StartFadeOut();

        while (SceneFadeManager.Instance.IsFadingOut)
        {

            isChangeArea = !isChangeArea;
            yield return null;
        }
        other.transform.position = pos.position;
        SceneFadeManager.Instance.StartFadeIn();
        PlayerInputHandler.ActivatePlayerControl();
    }
}