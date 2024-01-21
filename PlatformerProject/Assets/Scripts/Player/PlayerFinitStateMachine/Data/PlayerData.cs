using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;
    [Header("Wall Slide State")]
    public float wallSlideVelocity = 2f;
    [Header("Wall CLimb State")]
    public float wallClimbVelocity = 3f;
    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = .4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    [Header("Ledge CLimb State")]
    public Vector2 startOffset;
    public Vector2 stopOffset;
    public float climbSpeed = 1f;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.5f;

    public LayerMask whatIsGround;
}