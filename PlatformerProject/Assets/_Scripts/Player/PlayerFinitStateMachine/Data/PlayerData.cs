using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("---------PLAYERSTATE-------------------------------")]
    [Header("Move State")]
    public float movementVelocity = 10f;
    public float movementAcceleration = 0.4f;
    public PhysicsMaterial2D noFriction;
    public PhysicsMaterial2D fullFriction;
    [Header("Run State")]
    public float runVelocity = 15f;
    public float maxRunningTime = 1f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float minFallSpeed = -20f;
    public float fallSpeedDampingFactor = 0.75f;
    public float fallGravity = 8f;
    public float highFallTimeThreshold = 1f;
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
    [Header("Ledge Jump State")]
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
    [Header("Roll State")]
    public float rollCooldown = 1f;
    public float rollTime = .2f;
    public float rollVelocity = 30f;

    [Header("Crouch States")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = .8f;
    public float standColliderHeight = 1.6f;
    [Header("Attack States")]
    public float attackResetCooldown = 2f;
    public int energyGain = 5;
    [Header("Ground Attack States")]
    public int groundAttackCounter = 3;
    public AttackDetails[] groundAttackDetails;
    //special Attack
    public AttackDetails sprintAttackDetails;
    public AttackDetails holySlashAttack;
    [Header("Light Cut Attack")]
    public AttackDetails lightCutAttack;
    public float pushDuration = .1f;
    public float pushVelocity = 50f;
    public GameObject projectTile;
    [Header("Air Attack States")]
    public int airAttackCounter = 2;
    public AttackDetails[] airAttackDetails;
    //special Attack
    public AttackDetails downWardAttackDetails;
    #region AttackDetails Automatic Set
    private void OnValidate()
    {
        ValidateAttackDetails(ref groundAttackDetails, groundAttackCounter);
        ValidateAttackDetails(ref airAttackDetails, airAttackCounter);
    }

    private void ValidateAttackDetails(ref AttackDetails[] attackDetails, int counter)
    {
        if (attackDetails == null || attackDetails.Length != counter)
        {
            AttackDetails[] newAttackDetails = new AttackDetails[counter];

            for (int i = 0; i < counter; i++)
            {
                if (attackDetails != null && i < attackDetails.Length)
                {
                    newAttackDetails[i] = attackDetails[i];
                }
            }

            attackDetails = newAttackDetails;
        }
    }
    #endregion
    [Header("Turn States")]
    public float turnSlideSpeed = 10f;
    public float turnSlideDeceleration = 2f;
    [Header("Block State")]
    public float perfectBlockTime = .5f;
    public float blockRecoveryTime = 2f;
    public float blockKnockbackForce = 2f;
    public float blockKnockbackTime = 0.1f;
    public AttackDetails[] blockCounterAttackDetails;
    [Header("Land State")]
    public float sprintSlideVelocity = 15f;
    [Header("Heal State")]
    public int healingAmount = 1;
    [Header("Knockback State")]
    public float knockbackStrength = 10f;
    public Vector2 knockbackAngle = Vector2.one;
    [Header("---------OTHERDATAS-------------------------------")]

    [Header("Stamina Consume Amount")]
    public int blockStamina = 30;
    public int runStamina = 5;
    public int slideStamina = 50;
    public int dashStamina = 60;
    public int airAttackStamina = 25;
    [Header("Energy Consume Amount")]
    public int holySlashEnergy = 10;
    public int lightCutEnergy = 5;
    public int healEnergy = 20;
    [Header("Particle Prefabs")]
    public GameObject blockFX;
}