using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	private Movement Movement => movement ?? Core.GetCoreComponent(ref movement);
	private Movement movement;

	public FiniteStateMachine stateMachine;

	public D_Entity entityData;

	public Animator anim { get; private set; }
	public AnimationToStatemachine atsm { get; private set; }
	public int lastDamageDirection { get; private set; }
	public Core Core { get; private set; }
	public Vector2 TargetPosition { get; private set; }
	[SerializeField] private Transform wallCheck;
	[SerializeField] private Transform ledgeCheck;
	[SerializeField] private Transform playerCheck;
	[SerializeField] private Transform groundCheck;
	private float currentHealth;
	private float currentStunResistance;
	private float lastDamageTime;

	private Vector2 velocityWorkspace;

	protected bool isStunned;
	protected bool isDead;

	public virtual void Awake()
	{
		Core = GetComponentInChildren<Core>();

		currentHealth = entityData.maxHealth;
		currentStunResistance = entityData.stunResistance;

		anim = GetComponent<Animator>();
		atsm = GetComponent<AnimationToStatemachine>();

		stateMachine = new FiniteStateMachine();
	}

	public virtual void Update()
	{
		Core.LogicUpdate();
		stateMachine.currentState.LogicUpdate();

		anim.SetFloat("yVelocity", Movement.RB.velocity.y);

		if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
		{
			ResetStunResistance();
		}
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
		return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
	}

	public virtual bool CheckPlayerInMaxAgroRange()
	{
		return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
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

	public virtual void ResetStunResistance()
	{
		isStunned = false;
		currentStunResistance = entityData.stunResistance;
	}

	public virtual void OnDrawGizmos()
	{
		if (Core != null)
		{
			Gizmos.DrawLine(wallCheck.position, wallCheck.position + (transform.right * Movement.FacingDirection * entityData.wallCheckDistance));
			Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));

			Gizmos.DrawWireSphere(playerCheck.position + (transform.right * entityData.closeRangeActionDistance), 0.2f);
			Gizmos.DrawWireSphere(playerCheck.position + (transform.right * entityData.minAgroDistance), 0.2f);
			Gizmos.DrawWireSphere(playerCheck.position + (transform.right * entityData.maxAgroDistance), 0.2f);

			Gizmos.DrawWireSphere(transform.position, entityData.detectionRadius);
		}
	}
}