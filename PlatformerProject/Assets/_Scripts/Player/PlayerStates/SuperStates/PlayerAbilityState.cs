using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;
    protected Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    protected CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;
    protected ParticleManager ParticleManager => particleManager ?? core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;
    protected PlayerStats PlayerStats => playerStats ?? core.GetCoreComponent(ref playerStats);
    private PlayerStats playerStats;
    protected Combat Combat => combat ?? core.GetCoreComponent(ref combat);
    private Combat combat;
    protected bool isGrounded;
    protected bool isTouchingCeiling;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (CollisionSenses)//Check if has this component or not
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingCeiling = CollisionSenses.Ceiling;
        }
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAbilityDone)
        {
            if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}