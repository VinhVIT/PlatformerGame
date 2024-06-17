using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AreaTrigger : MonoBehaviour
{
    public static event Action<Collider2D> OnAreaChange;
    private Transform destination;
    private bool isChangeArea = false;
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
            OnAreaChange?.Invoke(nextAreaBound);
            StartCoroutine(ChangePosition(other, destination));
        }
    }
    private IEnumerator ChangePosition(Collider2D other, Transform pos)
    {
        float pauseTime = .5f;
        float transitionWaitingTime = 1f;
        PlayerInput playerInput = other.GetComponent<PlayerInput>();

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(pauseTime);
        //turn off player input and move player
        other.transform.position = pos.position;
        playerInput.enabled = false;
        Time.timeScale = 1f;
        isChangeArea = !isChangeArea;

        yield return new WaitForSeconds(transitionWaitingTime);
        //turn on player input
        playerInput.enabled = true;
    }
}
