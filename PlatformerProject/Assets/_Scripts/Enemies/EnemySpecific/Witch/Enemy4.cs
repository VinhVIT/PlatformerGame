using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : Entity
{
    public E4_MoveState moveState { get; private set; }
    public E4_IdleState idleState { get; private set; }
    public E4_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E4_LookForPlayerState lookForPlayerState { get; private set; }
    public E4_StunState StunState { get; private set; }
    public E4_DeadState DeadState { get; private set; }
    public E4_DodgeState DodgeState { get; private set; }
    public E4_RangedAttackState rangedAttackState { get; private set; }

    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_PlayerDetected PlayerDetectedStateData;
    [SerializeField] private D_LookForPlayer lookForPlayerStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] public D_DodgeState dodgeStateData;
    [SerializeField] private D_RangedAttackState rangedAttackStateData;

    [SerializeField] private Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();

        moveState = new E4_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E4_IdleState(this, stateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E4_PlayerDetectedState(this, stateMachine, "playerDetected", PlayerDetectedStateData, this);
        lookForPlayerState = new E4_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        StunState = new E4_StunState(this, stateMachine, "stun", stunStateData, this);
        DeadState = new E4_DeadState(this, stateMachine, "dead", deadStateData, this);
        DodgeState = new E4_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new E4_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

    }
}
