using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;
    protected bool isTouchingCeiling;
    protected Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    protected CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;
    protected ParticleManager ParticleManager => particleManager ?? core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;
    private bool JumpInput;
    private bool grabInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool dashInput;
    private bool slideInput;
    private bool attackInput;
    private bool spellCastInput;
    private int spellSlotInput;


    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)//Check if has this component or not
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
        player.SlideState.ResetCanSlide();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        JumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
        slideInput = player.InputHandler.SlideInput;
        attackInput = player.InputHandler.AttackInput;
        spellCastInput = player.InputHandler.SpellCastInput;
        spellSlotInput = player.InputHandler.SpellSlotInput;

        if (attackInput && !isTouchingCeiling)
        {
            player.AttackState.CheckToResetAttackCounter();
            stateMachine.ChangeState(player.AttackState);
        }
        else if (spellCastInput)
        {
            player.SpellCastState.SetSpellData(spellSlotInput);
            if (player.SpellCastState.CheckIfCanCastSpell())
            {
                stateMachine.ChangeState(player.SpellCastState);
            }
        }
        else if (JumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (slideInput && player.SlideState.CheckIfCanSlide())
        {
            stateMachine.ChangeState(player.SlideState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}