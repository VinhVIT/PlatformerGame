using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAirAttackState : PlayerAttackState
{
    private bool isDownWardAttack = false;
    private int yInput;
    public PlayerAirAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = playerData.airAttackCounter;
    }
    protected override int AttackCounter => playerData.airAttackCounter;

    protected override AttackDetails AttackDetails => playerData.airAttackDetails[attackCounter];
    public override void Enter()
    {
        base.Enter();
        player.SetGravity(0f);
        Movement.SetVelocityZero();

        if (isDownWardAttack)
        {
            PerformDownWardAttack();
        }
    }
    public override void Exit()
    {
        base.Exit();
        isDownWardAttack = false;
        player.ResetGravity();
        player.Anim.SetBool("isDownWardAttack", false);
        player.Anim.SetBool("downWardAttackHit", false);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        yInput = player.InputHandler.NormInputY;
        if (isDownWardAttack && CollisionSenses.Ground)
        {
            player.Anim.SetBool("downWardAttackHit",true);
        }
    }
    private void PerformDownWardAttack()
    {
        player.Anim.SetBool("isDownWardAttack", true);
    }
    public override void AnimationStartTrigger()
    {
        base.AnimationStartTrigger();
        if(isDownWardAttack) player.SetGravity(20);
    }
    public void CheckIsDownWardAttack(float yInput)
    {
        if (yInput < 0) isDownWardAttack = true;
    }

}
