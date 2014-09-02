using UnityEngine;
using System.Collections;

public class TestCollision : MonoBehaviour {


	[SerializeField] LayerMask whatIsGround;
	[SerializeField] LayerMask whatIsWall;

	[SerializeField] Transform groundCheck;
	[SerializeField] float groundedRadius = 0.2f;
	[SerializeField] float groundedRadiusPlus = 0.6f;

	
	[SerializeField] Transform ceilingCheck;
	[SerializeField] float ceilingRadius = 0.01f;

	
	[SerializeField] Transform wallCheck;
	[SerializeField] float wallTouchRadius = 0.2f;

	public bool grounded {get; private set;}
	public bool groundedPlus {get; private set;}
	public bool touchingCeiling {get; private set;}
	public bool touchingWall {get; private set;}


	void FixedUpdate () 
	{	
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		groundedPlus = Physics2D.OverlapCircle(groundCheck.position, groundedRadiusPlus, whatIsGround);
		touchingWall = Physics2D.OverlapCircle(wallCheck.position, wallTouchRadius, whatIsWall);
		touchingCeiling = Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = touchingWall ? Color.red : Color.white;
		Gizmos.DrawSphere(wallCheck.position, wallTouchRadius);
				
		Gizmos.color = groundedPlus ? Color.blue : Color.yellow;
		Gizmos.DrawSphere(groundCheck.position, groundedRadiusPlus);

		Gizmos.color = grounded ? Color.red : Color.white;
		Gizmos.DrawSphere(groundCheck.position, groundedRadius);

		Gizmos.color = touchingCeiling ? Color.red : Color.white;
		Gizmos.DrawSphere(ceilingCheck.position, ceilingRadius);

		Gizmos.color = Color.white;
	}
}
