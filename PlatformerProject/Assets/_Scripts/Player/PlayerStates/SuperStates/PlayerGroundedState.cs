using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;
    protected bool runInput;
    protected bool isTouchingCeiling;
    protected bool blockInput;
    protected Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    protected CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;
    protected ParticleManager ParticleManager => particleManager ?? core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;
    protected Combat Combat => combat ?? core.GetCoreComponent(ref combat);
    private Combat combat;
    private bool JumpInput;
    private bool grabInput;
    private bool healInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    protected bool dashInput;
    private bool rollInput;
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
        player.RollState.ResetCanRoll();
        player.BlockState.ResetCanBlock();
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
        runInput = player.InputHandler.RunInput;
        JumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        healInput = player.InputHandler.HealInput;
        dashInput = player.InputHandler.DashInput;
        blockInput = player.InputHandler.BlockInput;
        rollInput = player.InputHandler.RollInput;
        attackInput = player.InputHandler.AttackInput;
        spellCastInput = player.InputHandler.SpellCastInput;
        spellSlotInput = player.InputHandler.SpellSlotInput;

        if (attackInput && !isTouchingCeiling)
        {
            player.GroundAttackState.CheckIsSprintAttack(runInput);
            player.GroundAttackState.CheckToResetAttackCounter();
            stateMachine.ChangeState(player.GroundAttackState);
        }
        else if (healInput)
        {   
            Debug.Log("heal");
            stateMachine.ChangeState(player.HealState);
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
        else if (blockInput && player.BlockState.CheckIfCanBlock() && stateMachine.CurrentState != player.BlockState)
        {
            stateMachine.ChangeState(player.BlockState);
        }
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (rollInput && player.RollState.CheckIfCanRoll())
        {
            stateMachine.ChangeState(player.RollState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}