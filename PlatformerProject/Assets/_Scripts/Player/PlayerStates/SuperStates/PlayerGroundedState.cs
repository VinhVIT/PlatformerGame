using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    // Variables
    protected int xInput;
    protected int yInput;
    protected bool runInput;
    protected bool jumpInput;
    protected bool grabInput;
    protected bool healInput;
    protected bool dashInput;
    protected bool blockInput;
    protected bool rollInput;
    protected bool attackInput;
    protected bool buffInput;
    protected int buffSlotInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    protected bool isTouchingCeiling;

    // Component references
    protected Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    protected CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;
    protected ParticleManager ParticleManager => particleManager ?? core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;
    protected Combat Combat => combat ?? core.GetCoreComponent(ref combat);
    private Combat combat;
    protected PlayerStats PlayerStats => playerStats ?? core.GetCoreComponent(ref playerStats);
    private PlayerStats playerStats;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
            isTouchingCeiling = CollisionSenses.Ceiling;
        }
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.ResetCanDash();
        player.RollState.ResetCanRoll();
        player.BlockState.ResetCanBlock();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        GetInput();

        if (HandleAttack()) return;
        if (HandleHeal()) return;
        if (HandleBuff()) return;
        if (HandleJump()) return;
        if (HandleStateTransition()) return;
        if (HandleWallGrab()) return;
        if (HandleDash()) return;
        if (HandleRoll()) return;
    }

    private void GetInput()
    {
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        runInput = player.InputHandler.RunInput;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        healInput = player.InputHandler.HealInput;
        dashInput = player.InputHandler.DashInput;
        blockInput = player.InputHandler.BlockInput;
        rollInput = player.InputHandler.RollInput;
        attackInput = player.InputHandler.AttackInput;
        buffInput = player.InputHandler.BuffInput;
        buffSlotInput = player.InputHandler.BuffSlotInput;
    }

    private bool HandleAttack()
    {
        if (yInput == 0 && attackInput && !isTouchingCeiling)
        {
            player.GroundAttackState.CheckIsSprintAttack(runInput);
            player.GroundAttackState.CheckToResetAttackCounter();
            stateMachine.ChangeState(player.GroundAttackState);
            return true;
        }
        else if (yInput > 0 && attackInput && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.HolySlashState);
            return true;
        }
        else if (yInput < 0 && attackInput && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.LightCutAttackState);
            return true;
        }
        return false;
    }

    private bool HandleHeal()
    {
        if (healInput)
        {
            stateMachine.ChangeState(player.HealState);
            return true;
        }
        return false;
    }

    private bool HandleBuff()
    {
        if (buffInput)
        {
            stateMachine.ChangeState(player.BuffState);
            return true;
        }
        return false;
    }

    private bool HandleJump()
    {
        if (jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.JumpState);
            return true;
        }
        return false;
    }

    private bool HandleStateTransition()
    {
        if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
            return true;
        }
        return false;
    }

    private bool HandleWallGrab()
    {
        if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
            return true;
        }
        return false;
    }

    private bool HandleDash()
    {
        if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling
            && PlayerStats.Stamina.EnoughToUse(playerData.dashStamina))
        {
            stateMachine.ChangeState(player.DashState);
            return true;
        }
        return false;
    }

    private bool HandleRoll()
    {
        if (rollInput && player.RollState.CheckIfCanRoll()
            && PlayerStats.Stamina.EnoughToUse(playerData.slideStamina))
        {
            stateMachine.ChangeState(player.RollState);
            return true;
        }
        return false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
