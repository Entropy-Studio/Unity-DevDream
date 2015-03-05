using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TestCollision))]

public class PlayerController : MonoBehaviour 
{
	private TestCollision collision;
	
	[SerializeField] bool airControl = true;
	[SerializeField] bool switchWallSlide = true;
	[SerializeField] bool switchWallJump = true;
	[SerializeField] bool switchFallDamage = true;
	[SerializeField] bool switchCrouch = true;
	[SerializeField] bool switchBunnyJump = true;
	[SerializeField] bool switchSprint = true;
	
	[SerializeField]
	float maxVSpeed = 30f;
	[SerializeField]
	float minVSpeed = -30f;
	[SerializeField]
	float slideVSpeed = -5f;
	[SerializeField]
	float killSpeed = 25f;
	[SerializeField]
	float maxSpeed = 10f;
	[SerializeField]
	float jumpForce = 800f;
	[SerializeField]
	float bunnyJumpForce = 1200f;
	[SerializeField]
	float bunnyDelay = 1.0f;
	[SerializeField]
	float wallPushForce = 10f;
	[SerializeField]
	float recoveryLimitSpeed = 20f;
	[SerializeField]
	float sprintSpeed = 15f;
	
	float tempVSpeed;
	float tempHSpeed;
	
	bool facingRight = true;
	
	[Range(0, 1)]
	[SerializeField] float crouchSpeed = 0.33f;
	
	[SerializeField] LayerMask whatIsGround;
	[SerializeField] LayerMask whatIsWall;

    Animator anim;
    HashIDs hash;
	
	bool dead = false;
	bool alreadyJumped = false;
	bool bunnyJumpAllowed = false;
	bool noMove = false;

    [SerializeField]
    float health;
	
	void Awake()
	{
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

		tempVSpeed = minVSpeed;
		tempHSpeed = maxSpeed;
		collision = GetComponent<TestCollision>();
		
		anim = GetComponent<Animator>();
	}
	
	void Update()
	{
        health = GetComponent<HealthController>().health;

		if (GetComponent<Rigidbody2D>().velocity.y <= minVSpeed)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, minVSpeed);
		}
		
        if (GetComponent<Rigidbody2D>().velocity.y >= maxVSpeed)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, maxVSpeed);
		}
       
        if (health <= 0)
        {
            Death();
        }
	}
	
	void FixedUpdate()
	{
        anim.SetBool(hash.groundBool, collision.grounded);
		
		// Set the vertical animation
		anim.SetFloat(hash.vSpeedFloat, GetComponent<Rigidbody2D>().velocity.y);
		
		anim.SetBool(hash.deadBool, dead);
	}
	
	public void Death()
	{
		dead = true;
	}
	
	void FallDamage()
	{
		if(GetComponent<Rigidbody2D>().velocity.y <= -killSpeed && collision.groundedPlus)
		{
			Death ();
		}
	}
	
	public void Action(float move, bool crouch, bool jump, bool sprint)
	{	
		
		if (!dead)
		{
			if (switchFallDamage)
			{
				FallDamage();
			}
			if(!noMove)
			{
				if (switchCrouch)
				{
					Crouch(crouch);
				}
				Move(move, crouch);
				Jump(jump, crouch);
			}
			
			if (switchWallSlide)
			{
				WallSlide();
			}
			
			if (switchWallJump)
			{
				WallJump(jump);
			}
			
			if (switchBunnyJump)
			{
				BunnyJump();
			}
			if (switchSprint)
			{
				Sprint(sprint);
			}
			JumpRecoveryF();
		}		
	}
	
	void Crouch(bool crouch)
	{
		// If crouching, check to see if the character can stand up
		if(!crouch && anim.GetBool(hash.crouchBool))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if(collision.touchingCeiling)
				crouch = true;
		}
		
		// Set whether or not the character is crouching in the animator
        anim.SetBool(hash.crouchBool, crouch);
	}
	
	void Move(float move, bool crouch)
	{
		// only control the player if grounded or airControl is turned on
		if(collision.grounded || (collision.touchingWall && !collision.groundedPlus && Input.GetAxis("Horizontal") != 0))
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
            move = (anim.GetBool(hash.crouchBool) ? move * crouchSpeed : move); // ako je crouch tacno onda je move = move * crouchSpeed inace je move = move
			
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat(hash.speedFloat, Mathf.Abs(move));
			
			// maksimalna vertikalna brzina
			float vSpeed = Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.y, minVSpeed, maxVSpeed);
			// Move the character
			GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, vSpeed);
			
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		}
		if(airControl)
		{
			// maksimalna vertikalna brzina
			float vSpeed = Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.y, minVSpeed, maxVSpeed);
			
			GetComponent<Rigidbody2D>().AddForce(new Vector2(move*70f, 0f));
			if (GetComponent<Rigidbody2D>().velocity.x > maxSpeed)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, vSpeed);
			}
			
			if (GetComponent<Rigidbody2D>().velocity.x < -maxSpeed)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(-maxSpeed, vSpeed);
			}
			
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		}
	}
	
	void Jump (bool jump, bool crouch)
	{
		if(!crouch)
		{
			if (collision.grounded && jump) 
			{
				anim.SetBool(hash.groundBool, false);
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
				alreadyJumped = true;
			}
		}
	}
	
	void WallSlide ()
	{
		if (collision.touchingWall && !collision.groundedPlus && Input.GetAxis("Horizontal") != 0)
		{
			anim.SetBool(hash.wallSlideBool, true);
			minVSpeed = slideVSpeed;
		}
		else 
		{
            anim.SetBool(hash.wallSlideBool, false);
			minVSpeed = tempVSpeed;
		}
	}
	
	void WallJump(bool jump)
	{
		if(collision.touchingWall && jump && !collision.grounded)
		{
			if (facingRight)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(-wallPushForce, 0f);
			}
			else
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(wallPushForce, 0f);
			}
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
			
			Flip();
		}
	}
	
	void BunnyJump()
	{
		if(alreadyJumped && collision.groundedPlus && GetComponent<Rigidbody2D>().velocity.y < 0)
		{
			alreadyJumped = false;
			bunnyJumpAllowed = true;
			StartCoroutine(BunnyDelay(bunnyDelay));
		}
		
		if (bunnyJumpAllowed)
		{
			if (collision.grounded && Input.GetButtonUp("Jump"))
			{
				anim.SetBool(hash.groundBool, false);
				if (facingRight)
				{
					GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, 0f);
				}
				else
				{
					GetComponent<Rigidbody2D>().velocity = new Vector2(-maxSpeed, 0f);
				}
				
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, bunnyJumpForce));
				bunnyJumpAllowed = false;
			}
		}
	}
	
	void JumpRecoveryF()
	{
		if(GetComponent<Rigidbody2D>().velocity.y < -recoveryLimitSpeed && collision.groundedPlus)
		{
			StartCoroutine(JumpRecovery(1f));
		}
	}
	
	void Sprint(bool sprint)
	{
		if(!anim.GetBool(hash.crouchBool))
		{
			if (sprint)
			{
				maxSpeed = sprintSpeed;
			}
			else
			{
				maxSpeed = tempHSpeed;
			}
		}
	}
	
	void Flip ()
	{
		if (!dead)
		{
			// Switch the way the player is labelled as facing.
			facingRight = !facingRight;
			
			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}
		
	IEnumerator BunnyDelay(float sec)
	{
		noMove = true;
		anim.SetBool(hash.jumpRecoveryBool, true);
		yield return new WaitForSeconds(sec);
		bunnyJumpAllowed = false;
        anim.SetBool(hash.jumpRecoveryBool, false);
		noMove = false;
	}
	
	IEnumerator JumpRecovery(float sec)
	{
		if(!dead)
		{
			noMove = true;
            anim.SetBool(hash.jumpRecoveryBool, true);
			yield return new WaitForSeconds(sec);
            anim.SetBool(hash.jumpRecoveryBool, false);
			noMove = false;
		}
	}
}