using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDashState : State
{   
    protected D_BossDashState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isDectectingLedge;
    protected bool isDetectingWall;
    protected bool performCloseRangeAction;
    protected bool startDash;
    protected Movement Movement => movement ?? core.GetCoreComponent(ref movement);
    private Movement movement;
    protected CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;
    public BossDashState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_BossDashState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDectectingLedge = CollisionSenses.LedgeVertical;
        isDetectingWall = CollisionSenses.WallFront;

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }
    public override void Enter()
    {
        base.Enter();
        startDash = false;
        // Movement?.SetVelocityX(stateData.dashSpeed * Movement.FacingDirection);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(startDash)
        {
            Movement?.SetVelocityX(stateData.dashSpeed * Movement.FacingDirection);
        }
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        startDash = true;
    }
}
