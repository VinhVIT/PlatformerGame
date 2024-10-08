using System;
using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    //Behaviour
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerBlockState BlockState { get; private set; }
    public PlayerRollState RollState { get; private set; }
    public PlayerTurnState TurnState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerHurtState HurtState { get; private set; }
    public PlayerKnockbackState KnockbackState { get; private set; }
    public PlayerDeathState DeathState { get; private set; }
    //Enviroment Action
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerLedgeJumpState LedgeJumpState { get; private set; }

    //Attack
    public PlayerGroundAttackState GroundAttackState { get; private set; }
    public PlayerAirAttackState AirAttackState { get; private set; }
    public PlayerBlockCounterState BlockCounterState { get; private set; }
    public PlayerHolySlashState HolySlashState { get; private set; }
    public PlayerLightCutAttackState LightCutAttackState { get; private set; }
    //Abilities
    public PlayerHealState HealState { get; private set; }
    public PlayerBuffState BuffState { get; private set; }

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    private Combat Combat => combat ?? Core.GetCoreComponent(ref combat);
    private Combat combat;
    private PlayerStats PlayerStats => playerStats ?? Core.GetCoreComponent(ref playerStats);
    private PlayerStats playerStats;
    public PlayerSkills PlayerSkills { get; private set; }
    #endregion

    #region Other Variables
    private Vector2 workspace;
    private float originGravity;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        StateMachine = new PlayerStateMachine();
        PlayerSkills = new PlayerSkills();

        //Behaviour
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        RunState = new PlayerRunState(this, StateMachine, playerData, "run");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        BlockState = new PlayerBlockState(this, StateMachine, playerData, "block");
        RollState = new PlayerRollState(this, StateMachine, playerData, "roll");
        TurnState = new PlayerTurnState(this, StateMachine, playerData, "turn");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        HurtState = new PlayerHurtState(this, StateMachine, playerData, "hurt");
        KnockbackState = new PlayerKnockbackState(this, StateMachine, playerData, "knockback");
        DeathState = new PlayerDeathState(this, StateMachine, playerData, "death");

        //Enviroment Action
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "wallJump");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        LedgeJumpState = new PlayerLedgeJumpState(this, StateMachine, playerData, "inAir");

        //Attack
        GroundAttackState = new PlayerGroundAttackState(this, StateMachine, playerData, "groundAttack");
        AirAttackState = new PlayerAirAttackState(this, StateMachine, playerData, "airAttack");
        BlockCounterState = new PlayerBlockCounterState(this, StateMachine, playerData, "blockCounter");
        HolySlashState = new PlayerHolySlashState(this, StateMachine, playerData, "holySlash");
        LightCutAttackState = new PlayerLightCutAttackState(this, StateMachine, playerData, "lightCutState");

        //Abilities
        HealState = new PlayerHealState(this, StateMachine, playerData, "heal");
        BuffState = new PlayerBuffState(this, StateMachine, playerData, "buff");

    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");

        MovementCollider = GetComponent<BoxCollider2D>();

        originGravity = RB.gravityScale;

        StateMachine.Initialize(IdleState);

        PlayerSkills.OnSkillUnlocked += HandlerOnSkillUnlocked;
    }

    private void HandlerOnSkillUnlocked(object sender, PlayerSkills.UnlockSkillEventArgs e)
    {
        switch (e.skillType)
        {
            case PlayerSkills.SkillType.MaxHP_1:
                IncreaseMaxHP();
                break;
            case PlayerSkills.SkillType.Stamina_1:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion
    private void IncreaseMaxHP()
    {
        PlayerStats.Health.SetMaxValue(PlayerStats.Health.MaxValue + 1);
    }
    public bool CanUseHolySlash()
    {
        return PlayerSkills.IsSkillUnlocked(PlayerSkills.SkillType.HolySlash);
    }
    public bool CanUseLightCutter()
    {
        return PlayerSkills.IsSkillUnlocked(PlayerSkills.SkillType.LightCutter);
    }
    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }
    public void SetGravity(float amount) => RB.gravityScale = amount;
    public void ResetGravity() => RB.gravityScale = originGravity;
    public void ChangeLayer() => gameObject.layer = LayerMask.NameToLayer("NoCollisionWithEnemy");
    public void ResetLayer() => gameObject.layer = LayerMask.NameToLayer("Player");
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void AnimationStartTrigger() => StateMachine.CurrentState.AnimationStartTrigger();
    private void AnimationTurnOnFlipTrigger() => StateMachine.CurrentState.AnimationTurnOnFlipTrigger();
    private void AnimationTurnOffFlipTrigger() => StateMachine.CurrentState.AnimationTurnOffFlipTrigger();
    private void AnimationActionTrigger() => StateMachine.CurrentState.AnimationActionTrigger();
    #endregion
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            if (StateMachine.CurrentState == InAirState)
            {
                StateMachine.ChangeState(KnockbackState);
            }
            else
                StateMachine.ChangeState(HurtState);
            Combat.Damage(1);
        }
    }
}