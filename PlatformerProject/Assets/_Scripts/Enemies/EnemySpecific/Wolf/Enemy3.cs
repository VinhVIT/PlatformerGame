using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Entity
{   
    public E3_IdleState IdleState { get; private set; }
    public E3_MoveState MoveState { get; private set; }
    public E3_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E3_ChargeState ChargeState { get; private set; }
    public E3_LookForPlayerState lookForPlayerState { get; private set; }
    public E3_MeleeAttackState MeleeAttackState { get; private set; }
    public E3_StunState StunState { get; private set; }
    public E3_DeadState DeadState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetected playerDetectedData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_LookForPlayer lookForPlayerStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;


    [SerializeField] private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        MoveState = new E3_MoveState(this, stateMachine, "move", moveStateData, this);
        IdleState = new E3_IdleState(this, stateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E3_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        ChargeState = new E3_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E3_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        MeleeAttackState = new E3_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new E3_StunState(this, stateMachine, "stun", stunStateData, this);
        DeadState = new E3_DeadState(this, stateMachine, "dead", deadStateData, this);

    }
    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(MoveState);
    }

    protected override void OnCounterSuccessHandler()
    {
        stateMachine.ChangeState(StunState);
    }
    protected override void OnHealthZero()
    {
        stateMachine.ChangeState(DeadState);
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

}
