using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Entity
{
    private Stats Stats => stats ?? Core.GetCoreComponent(ref stats);
    private Stats stats;
    public E5_IdleState IdleState { get; private set; }
    public E5_MoveState MoveState { get; private set; }
    public E5_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E5_ChargeState ChargeState { get; private set; }
    public E5_LookForPlayerState lookForPlayerState { get; private set; }
    public E5_MeleeAttackState MeleeAttackState { get; private set; }
    public E5_StunState StunState { get; private set; }
    public E5_DeadState DeadState { get; private set; }

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

        MoveState = new E5_MoveState(this, stateMachine, "move", moveStateData, this);
        IdleState = new E5_IdleState(this, stateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E5_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        ChargeState = new E5_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E5_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        MeleeAttackState = new E5_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new E5_StunState(this, stateMachine, "stun", stunStateData, this);
        DeadState = new E5_DeadState(this, stateMachine, "dead", deadStateData, this);


    }
    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(MoveState);
        Stats.Health.OnCurrentValueZero += OnHealthZero;
    }

    private void OnHealthZero()
    {
        stateMachine.ChangeState(DeadState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
        Gizmos.DrawWireSphere(transform.position, entityData.detectionRadius);
    }

}
