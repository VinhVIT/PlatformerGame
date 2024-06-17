using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{   
    public static CameraFollowObject Instance;
    [Header("References")]
    [SerializeField] private GameObject player;
    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipRotationTime = 8.0f;
    private Coroutine turnCoroutine;
    private Movement playerMovement;
    private bool isFacingRight;

    private void Awake()
    {   
        Instance = this;
        playerMovement = player.GetComponentInChildren<Movement>();
        isFacingRight = playerMovement.FacingDirection != 1;
    }

    private void Update()
    {
        // make the cameraFollowObject follow the player's position
        transform.position = player.transform.position;
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < flipRotationTime)
        {
            elapsedTime += Time.deltaTime;
            // lerp the y rotation
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        isFacingRight = playerMovement.FacingDirection != 1;
        if (isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
