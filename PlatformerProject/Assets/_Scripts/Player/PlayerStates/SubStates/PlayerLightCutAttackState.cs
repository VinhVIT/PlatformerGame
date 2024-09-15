using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightCutAttackState : PlayerAttackState
{
    private bool canSetLightCutAttack;
    public PlayerLightCutAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = 0;
    }
    protected override int AttackCounter => 0;

    protected override AttackDetails AttackDetails => playerData.lightCutAttack;

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();
        canSetLightCutAttack = true;

        Combat.OnBeingAttacked += Combat_OnBeingAttacked;

    }

    private void Combat_OnBeingAttacked()
    {
        stateMachine.ChangeState(player.HurtState);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.InputHandler.AttackInputStop && canSetLightCutAttack)
        {
            player.ChangeLayer();
            player.Anim.SetBool("lightCutAttack", true);
            canSetLightCutAttack = false;
        }
    }

    private void PushForward()
    {
        Movement?.SetVelocityX(playerData.pushVelocity * Movement.FacingDirection);
        player.StartCoroutine(StoppushAfterDuration());
    }



    public override void Exit()
    {
        base.Exit();
        player.Anim.SetBool("lightCutAttack", false);
        player.ResetLayer();
        Combat.OnBeingAttacked -= Combat_OnBeingAttacked;
    }

    public override void AnimationTrigger()
    {
        PushForward();
        base.AnimationTrigger();
        player.StartCoroutine(SpawnProjectTile());
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        PlayerStats.Energy.Decrease(playerData.lightCutEnergy);
    }
    private IEnumerator StoppushAfterDuration()
    {
        yield return new WaitForSeconds(playerData.pushDuration);
        Movement?.SetVelocityX(0f);
    }
    private IEnumerator SpawnProjectTile()
    {
        yield return new WaitForSeconds(0.1f);
        Object.Instantiate(playerData.projectTile, player.transform.position + player.transform.right * 1.5f, player.transform.rotation);
    }
}
