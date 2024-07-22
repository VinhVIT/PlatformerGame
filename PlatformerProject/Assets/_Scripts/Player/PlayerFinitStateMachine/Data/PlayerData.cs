using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;
    public float movementAcceleration = 0.4f;
    public PhysicsMaterial2D noFriction;
    public PhysicsMaterial2D fullFriction;
    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float minFallSpeed = -15f; 
    public float fallSpeedDampingFactor = 0.75f;
    public float variableJumpHeightMultiplier = 0.5f;
    [Header("Wall Slide State")]
    public float wallSlideVelocity = 2f;
    [Header("Wall CLimb State")]
    public float wallClimbVelocity = 3f;
    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = .4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    [Header("Ledge Grab State")]
    public Vector2 startOffset;//startPos when detectedLedge
    public Vector2 stopOffset;//endPos when finish LedgeGrab
    [Header("Ledge Grab State")]
    public float ledgeJumpVelocity = 20f;
    public float ledgeJumpTime = .2f;

    [Header("Dash State")]
    public float dashCooldown = 1f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = .25f;
    public float dashTime = .2f;
    public float dashVelocity = 30f;
    public float drag = 10f;//airDrag affect gravity when dash
    public float dashEndYMultiplier = .2f;
    public float distBetweenAfterImages = .5f;
    [Header("Slide State")]
    public float slideCooldown = 1f;
    public float slideTime = .2f;
    public float slideVelocity = 30f;

    [Header("Crouch States")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = .8f;
    public float standColliderHeight = 1.6f;
    [Header("Attack States")]
    public int attackCounter = 3;
    public int[] attackDamage;
    public float attackResetCooldown = 2f;
    public float[] attackMovementSpeed;
    public float[] knockbackStrength;
    public Vector2[] knockbackAngle;
    [Header("Particle Prefabs")]
    public GameObject dustJumpParticle;
    public GameObject dustFallParticle;
    [Header("Spell SO")]
    public SpellData[] spellDatas;
}