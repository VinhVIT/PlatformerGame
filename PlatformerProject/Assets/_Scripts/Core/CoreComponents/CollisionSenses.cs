using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{

	private Movement Movement => movement ?? core.GetCoreComponent(ref movement);
	private Movement movement;

	#region Check Transforms

	public Transform GroundCheck
	{
		get => GenericNotImplementedError<Transform>.TryGet(groundCheck, core.transform.parent.name);
		private set => groundCheck = value;
	}
	public Transform WallCheck
	{
		get => GenericNotImplementedError<Transform>.TryGet(wallCheck, core.transform.parent.name);
		private set => wallCheck = value;
	}
	public Transform LedgeCheckHorizontal
	{
		get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckHorizontal, core.transform.parent.name);
		private set => ledgeCheckHorizontal = value;
	}
	public Transform LedgeCheckVertical
	{
		get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckVertical, core.transform.parent.name);
		private set => ledgeCheckVertical = value;
	}
	public Transform CeilingCheck
	{
		get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, core.transform.parent.name);
		private set => ceilingCheck = value;
	}
	public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
	public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
	public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }


	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform wallCheck;
	[SerializeField] private Transform ledgeCheckHorizontal;
	[SerializeField] private Transform ledgeCheckVertical;
	[SerializeField] private Transform ceilingCheck;
	[SerializeField] private Transform enemyCheck;
	[SerializeField] private float groundCheckRadius;
	[SerializeField] private float wallCheckDistance;
	[SerializeField] private float enemyCheckDistance;
	[SerializeField] private LayerMask whatIsGround;
	[SerializeField] private LayerMask whatCanDetected;
	#endregion

	public bool Ceiling
	{
		get => Physics2D.OverlapCircle(CeilingCheck.position, groundCheckRadius, whatIsGround);
	}

	public bool Ground
	{
		get => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);
	}

	public bool WallFront
	{
		get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
	}

	public bool LedgeHorizontal
	{
		get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
	}

	public bool LedgeVertical
	{
		get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround);
	}

	public bool WallBack
	{
		get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround);
	}
	public Vector3 EnemyInRange
	{
		get
		{
			RaycastHit2D hit = Physics2D.BoxCast(enemyCheck.position, new Vector2(0.5f, 3f), 0, Vector2.right * Movement.FacingDirection, enemyCheckDistance, whatCanDetected);
			if (hit.collider != null)
			{
				//calculate center of enemy feet
				Vector2 enemyCenterBottomPosition = hit.collider.bounds.center;
				enemyCenterBottomPosition.y -= hit.collider.bounds.size.y / 2f;

				return enemyCenterBottomPosition;
			}
			else
			{
				return Vector3.zero;
			}
		}
	}
	#region BoxCastGizMos
	// void OnDrawGizmos()
	// {
	// 	DrawBoxCast(enemyCheck.position, new Vector2(.5f, 3), Vector2.right * Movement.FacingDirection, enemyCheckDistance);
	// }

	// void DrawBoxCast(Vector2 origin, Vector2 size, Vector2 direction, float distance)
	// {
	// 	Vector2 halfSize = size / 2;
	// 	Vector2 right = Vector2.Perpendicular(direction) * halfSize.y;
	// 	Vector2 up = direction * halfSize.x;

	// 	Vector2 topRight = origin + right + up;
	// 	Vector2 topLeft = origin - right + up;
	// 	Vector2 bottomRight = origin + right - up;
	// 	Vector2 bottomLeft = origin - right - up;

	// 	Gizmos.color = Color.red;

	// 	// Draw the initial rectangle
	// 	Gizmos.DrawLine(topRight, topLeft);
	// 	Gizmos.DrawLine(topLeft, bottomLeft);
	// 	Gizmos.DrawLine(bottomLeft, bottomRight);
	// 	Gizmos.DrawLine(bottomRight, topRight);

	// 	// Draw the extended rectangle in the direction
	// 	Vector2 topRightEnd = topRight + direction * distance;
	// 	Vector2 topLeftEnd = topLeft + direction * distance;
	// 	Vector2 bottomRightEnd = bottomRight + direction * distance;
	// 	Vector2 bottomLeftEnd = bottomLeft + direction * distance;

	// 	Gizmos.DrawLine(topRightEnd, topLeftEnd);
	// 	Gizmos.DrawLine(topLeftEnd, bottomLeftEnd);
	// 	Gizmos.DrawLine(bottomLeftEnd, bottomRightEnd);
	// 	Gizmos.DrawLine(bottomRightEnd, topRightEnd);

	// 	// Draw connecting lines
	// 	Gizmos.DrawLine(topRight, topRightEnd);
	// 	Gizmos.DrawLine(topLeft, topLeftEnd);
	// 	Gizmos.DrawLine(bottomRight, bottomRightEnd);
	// 	Gizmos.DrawLine(bottomLeft, bottomLeftEnd);
	// }
	#endregion
}