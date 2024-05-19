using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NO NEED YET
public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.SetVelocityY(playerData.wallClimbVelocity);

        // if(yInput != 1 && !isExitingState)
        // {
        //     stateMachine.ChangeState(player.WallGrabState);
        // }
    }
}
