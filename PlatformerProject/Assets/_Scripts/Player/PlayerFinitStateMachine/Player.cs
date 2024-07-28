using UnityEngine;


public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerLedgeJumpState LedgeJumpState { get; private set; }

    public PlayerDashState DashState { get; private set; }
    public PlayerBlockState BlockState { get; private set; }
    public PlayerBlockCounterState BlockCounterState { get; private set; }


    public PlayerRollState RollState { get; private set; }
    public PlayerTurnState TurnState { get; private set; }

    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerGroundAttackState GroundAttackState { get; private set; }
    public PlayerAirAttackState AirAttackState { get; private set; }
    public PlayerSpellCastState SpellCastState { get; private set; }

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
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

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "wallJump");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        LedgeJumpState = new PlayerLedgeJumpState(this, StateMachine, playerData, "inAir");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        BlockState = new PlayerBlockState(this, StateMachine, playerData, "block");
        BlockCounterState = new PlayerBlockCounterState(this, StateMachine, playerData, "blockCounter");
        RollState = new PlayerRollState(this, StateMachine, playerData, "roll");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        GroundAttackState = new PlayerGroundAttackState(this, StateMachine, playerData, "groundAttack");
        AirAttackState = new PlayerAirAttackState(this, StateMachine, playerData, "airAttack");

        SpellCastState = new PlayerSpellCastState(this, StateMachine, playerData, "spellCast");
        TurnState = new PlayerTurnState(this, StateMachine, playerData, "turn");
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
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void AnimationStartTrigger() => StateMachine.CurrentState.AnimationStartTrigger();
    private void AnimationTurnOnFlipTrigger() => StateMachine.CurrentState.AnimationTurnOnFlipTrigger();
    private void AnimationTurnOffFlipTrigger() => StateMachine.CurrentState.AnimationTurnOffFlipTrigger();
    private void AnimationActionTrigger() => StateMachine.CurrentState.AnimationActionTrigger();
    #endregion
}