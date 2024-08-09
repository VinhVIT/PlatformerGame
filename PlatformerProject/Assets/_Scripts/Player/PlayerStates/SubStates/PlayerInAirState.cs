using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;
    private PlayerStats PlayerStats => playerStats ?? core.GetCoreComponent(ref playerStats);
    private PlayerStats playerStats;
    protected Combat Combat => combat ?? core.GetCoreComponent(ref combat);
    private Combat combat;
    //Input
    private int xInput;
    private int yInput;
    private bool jumpInput;
    private bool runInput;
    private bool grabInput;
    private bool dashInput;
    private bool attackInput;
    private bool jumpInputStop;
    //Check
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool wallJumpCoyoteTime;
    private float startWallJumpCoyoteTime;
    private bool coyoteTime;
    private bool isJumping;
    private bool isTouchingLedge;
    private float fallSpeedYDampingChangeThreshold;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingWallBack = CollisionSenses.WallBack;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
        }

        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }
    public override void Enter()
    {
        base.Enter();
        fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;
        player.RunState.CheckIfShouldSprintJump();

        Combat.OnBeingAttacked += HandlerOnBeingAttacked;
    }
    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
        player.Anim.ResetTrigger("resetSprintJump");
        Combat.OnBeingAttacked -= HandlerOnBeingAttacked;

    }

    private void HandlerOnBeingAttacked()
    {
        stateMachine.ChangeState(player.KnockbackState);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        GetInput();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        LimitFallSpeed();

        CheckJumpMultiplier();
        ResetSprintJump();

        if (HandleAttackInput()) return;
        if (HandleLanding()) return;
        if (HandleWallInteractions()) return;
        if (HandleJumpActions()) return;
        if (HandleDash()) return;

        HandleMovement();
        HandleCamera();
    }
    #region Handler Func
    private void GetInput()
    {
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        runInput = player.InputHandler.RunInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
        attackInput = player.InputHandler.AttackInput;
    }
    private bool HandleAttackInput()
    {
        if (attackInput && PlayerStats.Stamina.EnoughToUse(playerData.airAttackStamina))
        {
            player.AirAttackState.CheckIsDownWardAttack(yInput);
            player.AirAttackState.CheckToResetAttackCounter();
            stateMachine.ChangeState(player.AirAttackState);
            return true;
        }
        return false;
    }

    private bool HandleLanding()
    {
        if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
            return true;
        }
        return false;
    }

    private bool HandleWallInteractions()
    {
        if (isTouchingWall && !isTouchingLedge && !isGrounded && movement.CurrentVelocity.y < 0)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
            return true;
        }
        if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
            return true;
        }
        return false;
    }

    private bool HandleJumpActions()
    {
        if (xInput == Movement.FacingDirection && jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime) && isTouchingLedge)
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = CollisionSenses.WallFront;
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
            return true;
        }
        if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
            return true;
        }
        return false;
    }

    private bool HandleDash()
    {
        if (dashInput && player.DashState.CheckIfCanDash()
        && PlayerStats.Stamina.EnoughToUse(playerData.dashStamina))
        {
            stateMachine.ChangeState(player.DashState);
            return true;
        }
        return false;
    }

    private void HandleMovement()
    {
        Movement?.CheckIfShouldFlip(xInput);
        if (runInput && PlayerStats.Stamina.EnoughToUse(playerData.blockStamina))
        {
            Movement?.SetVelocityX(playerData.runVelocity * xInput);
        }
        else
        {
            Movement?.SetVelocityX(playerData.movementVelocity * xInput);
        }
        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
    }

    private void HandleCamera()
    {
        if (Movement.CurrentVelocity.y < fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }
        if (Movement.CurrentVelocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    #endregion
    #region Other Func
    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }

        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }
    private void ResetSprintJump()
    {
        if (dashInput || attackInput || CollisionSenses.Ground)
        {
            player.Anim.SetBool("resetSprintJump", true);
            player.Anim.SetBool("sprintJump", false);
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }
    private void LimitFallSpeed()
    {
        if (Movement.CurrentVelocity.y < playerData.minFallSpeed)
        {
            Movement.SetVelocityY(Movement.CurrentVelocity.y * playerData.fallSpeedDampingFactor);
        }
    }
    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;
    #endregion
}