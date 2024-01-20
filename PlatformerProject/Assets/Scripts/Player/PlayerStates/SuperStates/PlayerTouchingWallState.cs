using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected int xInput;
    protected int yInput;
    protected bool grabInput;
    protected bool isGrounded;
    protected bool isTouchingWall;
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        grabInput = player.InputHandler.grabInput;


        if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if(!isTouchingWall || (xInput != player.FacingDirection && !grabInput))
        {
            stateMachine.ChangeState(player.InAirState);
        }
    }
}
