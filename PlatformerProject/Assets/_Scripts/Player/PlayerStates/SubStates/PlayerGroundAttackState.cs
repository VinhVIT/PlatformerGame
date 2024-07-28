using UnityEngine;

public class PlayerGroundAttackState : PlayerAttackState
{
    public PlayerGroundAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = playerData.groundAttackCounter;
    }
    protected override int AttackCounter => playerData.groundAttackCounter;
    protected override AttackDetails AttackDetails => playerData.groundAttackDetails[attackCounter];
    public override void Enter()
    {
        base.Enter();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

}
