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
    public PlayerLedgeGrabState LedgeGrabState { get; private set; }
    public PlayerLedgeJumpState LedgeJumpState { get; private set; }

    public PlayerDashState DashState { get; private set; }
    public PlayerSlideState SlideState { get; private set; }

    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
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
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeGrabState = new PlayerLedgeGrabState(this, StateMachine, playerData, "ledgeGrab");
        LedgeJumpState = new PlayerLedgeJumpState(this, StateMachine, playerData, "inAir");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        SlideState = new PlayerSlideState(this, StateMachine, playerData, "slide");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        AttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        SpellCastState = new PlayerSpellCastState(this, StateMachine, playerData, "spellCast");

    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        MovementCollider = GetComponent<BoxCollider2D>();

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

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void AnimtionStartTrigger() => StateMachine.CurrentState.AnimationStartTrigger();
    private void AniamtionTurnOnFlipTrigger() => AttackState.AnimationTurnOnFlipTrigger();
    private void AnimationTurnOffFlipTrigger() => AttackState.AnimationTurnOffFlipTrigger();
    private void AnimationActionTrigger() => AttackState.AnimationActionTrigger();
    #endregion
}