using System;
using System.Diagnostics;

public class PlayerMoveState : PlayerGroundedState
{   
    public event Action OnMovementVelocityChanged;
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName)
    {
        movementVelocity = playerData.movementVelocity;

    }
    private float movementVelocity;
    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (stateMachine.CurrentState != player.RollState)
            {
                Movement?.CheckIfShouldFlip(xInput);
                Movement?.SetVelocityX(movementVelocity * xInput);
            }
            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (runInput && PlayerStats.Stamina.EnoughToUse(playerData.runStamina))
            {
                stateMachine.ChangeState(player.RunState);
            }
        }
    }
    public void IncreaseMovementVelocity(float amount)
    {

        movementVelocity += amount;
        OnMovementVelocityChanged?.Invoke();
    }

    public float GetMovementVelocity() => movementVelocity;
}
