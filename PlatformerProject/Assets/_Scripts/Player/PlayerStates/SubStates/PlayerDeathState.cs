using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerAbilityState
{
    public PlayerDeathState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Combat.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityZero();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        // player.gameObject.SetActive(false);
    }
}
