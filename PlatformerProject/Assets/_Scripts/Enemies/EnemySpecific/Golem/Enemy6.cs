using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6 : Entity
{   
    public E6_IdleState IdleState { get; private set; }
    public E6_MoveState MoveState { get; private set; }
    public E6_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E6_ChargeState ChargeState { get; private set; }
    public E6_LookForPlayerState lookForPlayerState { get; private set; }
    public E6_MeleeAttackState MeleeAttackState { get; private set; }
    public E6_StunState StunState { get; private set; }
    public E6_DeadState DeadState { get; private set; }

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

        MoveState = new E6_MoveState(this, stateMachine, "move", moveStateData, this);
        IdleState = new E6_IdleState(this, stateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E6_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        ChargeState = new E6_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E6_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        MeleeAttackState = new E6_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new E6_StunState(this, stateMachine, "stun", stunStateData, this);
        DeadState = new E6_DeadState(this, stateMachine, "dead", deadStateData, this);


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
