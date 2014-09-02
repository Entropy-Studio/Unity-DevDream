using UnityEngine;
using System.Collections;

public class TestCollision : MonoBehaviour {

	[SerializeField] bool drawGizmos = true;

	[SerializeField] LayerMask whatIsGround;
	[SerializeField] LayerMask whatIsWall;

	[SerializeField] Transform groundCheck;
	[SerializeField] float groundedRadius = 0.2f;

	[SerializeField] Transform startCastGround;
	[SerializeField] Transform endCastGround;

	
	[SerializeField] Transform ceilingCheck;
	[SerializeField] float ceilingRadius = 0.01f;

	
	[SerializeField] Transform wallCheck;
	[SerializeField] float wallTouchRadius = 0.2f;

	public bool grounded {get; private set;}
	public bool groundedPlus {get; private set;}
	public bool touchingCeiling {get; private set;}
	public bool touchingWall {get; private set;}


	void Update () 
	{	
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		groundedPlus = Physics2D.Linecast(startCastGround.position, endCastGround.position, whatIsGround);
		touchingWall = Physics2D.OverlapCircle(wallCheck.position, wallTouchRadius, whatIsWall);
		touchingCeiling = Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround);
	}

	void OnDrawGizmos()
	{
		if(drawGizmos)
		{
			Gizmos.color = touchingWall ? Color.red : Color.white;
			Gizmos.DrawSphere(wallCheck.position, wallTouchRadius);
			
			if(groundedPlus)
			{
				Debug.DrawLine(startCastGround.position, endCastGround.position, Color.red);
			}else
			{
				Debug.DrawLine(startCastGround.position, endCastGround.position, Color.blue);
			}
			
			Gizmos.color = grounded ? Color.red : Color.white;
			Gizmos.DrawSphere(groundCheck.position, groundedRadius);
			
			Gizmos.color = touchingCeiling ? Color.red : Color.white;
			Gizmos.DrawSphere(ceilingCheck.position, ceilingRadius);
			
			Gizmos.color = Color.white;
		}
	}
}
