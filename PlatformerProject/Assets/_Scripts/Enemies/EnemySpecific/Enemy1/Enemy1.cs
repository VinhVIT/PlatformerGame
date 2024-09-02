using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{   
    private Stats Stats => stats ?? Core.GetCoreComponent(ref stats);
    private Stats stats;

    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }
    public E1_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E1_ChargeState ChargeState { get; private set; }
    public E1_LookForPlayerState lookForPlayerState { get; private set; }
    public E1_MeleeAttackState MeleeAttackState { get; private set; }
    public E1_StunState StunState { get; private set; }
    public E1_DeadState DeadState { get; private set; }

    [SerializeField]private D_IdleState idleStateData;
    [SerializeField]private D_MoveState moveStateData;
    [SerializeField]private D_PlayerDetected playerDetectedData;
    [SerializeField]private D_ChargeState chargeStateData;
    [SerializeField]private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]private D_MeleeAttack meleeAttackStateData;
    [SerializeField]private D_StunState stunStateData;
    [SerializeField]private D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        moveState = new E1_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E1_IdleState(this, stateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E1_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        ChargeState = new E1_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E1_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        MeleeAttackState = new E1_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new E1_StunState(this, stateMachine, "stun", stunStateData, this);
        DeadState = new E1_DeadState(this, stateMachine, "dead", deadStateData, this);

    }

    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(moveState);
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
    }

}
