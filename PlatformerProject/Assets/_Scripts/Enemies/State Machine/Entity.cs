using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Entity : MonoBehaviour
{
	protected Movement Movement => movement ?? Core.GetCoreComponent(ref movement);
	private Movement movement;
	protected Combat Combat => combat ?? Core.GetCoreComponent(ref combat);
	private Combat combat;
	protected Stats Stats => stats ?? Core.GetCoreComponent(ref stats);
	private Stats stats;
	public FiniteStateMachine stateMachine;
	public D_Entity entityData;
	public Animator anim { get; private set; }
	public AnimationToStatemachine atsm { get; private set; }
	public CinemachineImpulseSource ImpulseSource { get; private set; }
	public int lastDamageDirection { get; private set; }
	public Core Core { get; private set; }
	public Vector2 TargetPosition { get; private set; }
	[SerializeField] private Transform wallCheck;
	[SerializeField] private Transform ledgeCheck;
	[SerializeField] private Transform playerCheck;
	[SerializeField] private Transform groundCheck;
	private float lastDamageTime;
	private Vector2 velocityWorkspace;
	protected bool isStunned;
	protected bool isDead;

	public virtual void Awake()
	{
		Core = GetComponentInChildren<Core>();


		anim = GetComponent<Animator>();
		atsm = GetComponent<AnimationToStatemachine>();
		ImpulseSource = GetComponent<CinemachineImpulseSource>();

		stateMachine = new FiniteStateMachine();
	}
	public virtual void Start()
	{
		Combat.OnBeingAttacked += OnBeingAttackHandler;
		Stats.Health.OnCurrentValueZero += OnHealthZero;
		EventManager.Player.OnCounterSuccess += OnCounterSuccessHandler;
	}
	public virtual void OnDestroy()
	{	
		Combat.OnBeingAttacked -= OnBeingAttackHandler;
		Stats.Health.OnCurrentValueZero -= OnHealthZero;
		EventManager.Player.OnCounterSuccess -= OnCounterSuccessHandler;
	}
	protected virtual void OnBeingAttackHandler()
	{
		if (ImpulseSource == null) return;

		CameraManager.Instance.ShakeWithProfile(entityData.profile, ImpulseSource);
	}
	protected virtual void OnCounterSuccessHandler()
	{
		// stateMachine.ChangeState(StunState);
	}

	protected virtual void OnHealthZero()
	{
		// stateMachine.ChangeState(DeadState);
	}
	public virtual void Update()
	{
		Core.LogicUpdate();
		stateMachine.currentState.LogicUpdate();

		anim.SetFloat("yVelocity", Movement.RB.velocity.y);
	}

	public virtual void FixedUpdate()
	{
		stateMachine.currentState.PhysicsUpdate();
	}
	public virtual bool CheckPlayerInDetectionRange()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, entityData.detectionRadius, entityData.whatIsPlayer);

		foreach (Collider2D collider in colliders)
		{
			Vector2 directionToCollider = collider.transform.position - transform.position;
			float angle = Vector2.Angle(transform.right, directionToCollider);

			//detect when first half circle range
			if (angle <= 90f)
			{
				TargetPosition = collider.transform.position;
				return true;
			}
		}

		return false;
	}
	public virtual bool CheckPlayerInMinAgroRange()
	{
		Vector2 boxSize = new Vector2(entityData.minAgroDistance, entityData.agroSize);

		Vector2 boxCenter = (Vector2)playerCheck.position + (Vector2)transform.right * (entityData.minAgroDistance / 2);

		return Physics2D.OverlapBox(boxCenter, boxSize, 0f, entityData.whatIsPlayer);
	}

	public virtual bool CheckPlayerInMaxAgroRange()
	{
		Vector2 boxSize = new Vector2(entityData.maxAgroDistance, entityData.agroSize);

		Vector2 boxCenter = (Vector2)playerCheck.position + (Vector2)transform.right * (entityData.maxAgroDistance / 2);

		return Physics2D.OverlapBox(boxCenter, boxSize, 0f, entityData.whatIsPlayer);
	}

	public virtual bool CheckPlayerInCloseRangeAction()
	{
		return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
	}

	public virtual void DamageHop(float velocity)
	{
		velocityWorkspace.Set(Movement.RB.velocity.x, velocity);
		Movement.RB.velocity = velocityWorkspace;
	}

	private void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
	private void AnimtionFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

	public virtual void OnDrawGizmos()
	{
		if (Core != null)
		{
			Gizmos.DrawLine(wallCheck.position, wallCheck.position + (transform.right * Movement.FacingDirection * entityData.wallCheckDistance));
			Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));

			Vector2 minAgroBoxCenter = (Vector2)playerCheck.position + (Vector2)transform.right * (entityData.minAgroDistance / 2);
			Vector2 minAgroBoxSize = new Vector2(entityData.minAgroDistance, entityData.agroSize);
			Gizmos.DrawWireCube(minAgroBoxCenter, minAgroBoxSize);

			Vector2 maxAgroBoxCenter = (Vector2)playerCheck.position + (Vector2)transform.right * (entityData.maxAgroDistance / 2);
			Vector2 maxAgroBoxSize = new Vector2(entityData.maxAgroDistance, entityData.agroSize);
			Gizmos.DrawWireCube(maxAgroBoxCenter, maxAgroBoxSize);
		}
	}
}