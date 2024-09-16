using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Entity
{
    public B1_BossLandState BossLandState { get; private set; }
    public B1_BossLaughState BossLaughState { get; private set; }
    public B1_BossEngageState BossEngageState { get; private set; }
    public B1_BossDashState BossDashState { get; private set; }
    public B1_IdleState idleState { get; private set; }
    public B1_MoveState moveState { get; private set; }
    public B1_PlayerDetectedState PlayerDetectedState { get; private set; }
    public B1_ChargeState ChargeState { get; private set; }
    public B1_LookForPlayerState lookForPlayerState { get; private set; }
    public B1_MeleeAttackState MeleeAttackState { get; private set; }
    public B1_StunState StunState { get; private set; }
    public B1_DeadState DeadState { get; private set; }

    [SerializeField] private D_BossLandState bossLandStateData;
    [SerializeField] private D_BossLaughState bossLaughStateData;
    [SerializeField] private D_BossEngageState bossEngageStateData;
    [SerializeField] private D_BossDashState bossDashStateData;
    [SerializeField] private D_IdleState idleStateData;
    // [SerializeField]private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetected playerDetectedData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_LookForPlayer lookForPlayerStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;

    [SerializeField] private Transform meleeAttackPosition;
    public bool IsEngaged { get; private set; } = false;
    public override void Awake()
    {
        base.Awake();

        BossLandState = new B1_BossLandState(this, stateMachine, "land", bossLandStateData, this);
        BossLaughState = new B1_BossLaughState(this, stateMachine, "laugh", bossLaughStateData, this);
        BossEngageState = new B1_BossEngageState(this, stateMachine, "engage", bossEngageStateData, this);
        BossDashState = new B1_BossDashState(this, stateMachine, "dash", bossDashStateData, this);
        // moveState = new B1_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new B1_IdleState(this, stateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new B1_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        ChargeState = new B1_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new B1_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        MeleeAttackState = new B1_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new B1_StunState(this, stateMachine, "stun", stunStateData, this);
        DeadState = new B1_DeadState(this, stateMachine, "dead", deadStateData, this);

    }

    public override void Start()
    {
        base.Start();
        Movement?.Flip();
        stateMachine.Initialize(BossLandState);
    }
    
    protected override void OnCounterSuccessHandler()
    {
        stateMachine.ChangeState(StunState);
    }
    protected override void OnHealthZero()
    {
        stateMachine.ChangeState(DeadState);
    }
    public override void Update()
    {
        base.Update();
        if (Stats.Health.CurrentValue < Stats.Health.MaxValue / 2 && !IsEngaged)
        {
            IsEngaged = true;
            stateMachine.ChangeState(BossEngageState);
        }
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

}
