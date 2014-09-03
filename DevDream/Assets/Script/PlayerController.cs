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

	// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //
	// ako menjas maxSpeed obavezno promeni i u Sprint funkciji!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //
	// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

	bool facingRight = true;

	[Range(0, 1)]
	[SerializeField] float crouchSpeed = 0.33f;

	[SerializeField] LayerMask whatIsGround;
	[SerializeField] LayerMask whatIsWall;

	Animator anim;	

	bool dead = false;
	bool alreadyJumped = false;
	bool bunnyJumpAllowed = false;
	bool noMove = false;
	
	void Awake()
	{
		collision = GetComponent<TestCollision>();

		anim = GetComponent<Animator>();
	}

	void Update()
	{

	}

	void FixedUpdate()
	{
		anim.SetBool("Ground", collision.grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
		
		anim.SetBool("Dead", dead);
	}

	void Death()
	{
		dead = true;
	}

	void FallDamage()
	{
		if(rigidbody2D.velocity.y <= -killSpeed && collision.groundedPlus)
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
		if(!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if(collision.touchingCeiling)
				crouch = true;
		}
		
		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);
	}

	void Move(float move, bool crouch)
	{
		// only control the player if grounded or airControl is turned on
		if(collision.grounded || (collision.touchingWall && !collision.groundedPlus && Input.GetAxis("Horizontal") != 0))
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (anim.GetBool("Crouch") ? move * crouchSpeed : move); // ako je crouch tacno onda je move = move * crouchSpeed inace je move = move
			
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(move));
			
			// maksimalna vertikalna brzina
			float vSpeed = Mathf.Clamp(rigidbody2D.velocity.y, minVSpeed, maxVSpeed);
			// Move the character
			rigidbody2D.velocity = new Vector2(move * maxSpeed, vSpeed);
			
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
			rigidbody2D.AddForce(new Vector2(move*100f, 0f));
			if (rigidbody2D.velocity.x > maxSpeed)
			{
				rigidbody2D.velocity = new Vector2(maxSpeed, rigidbody2D.velocity.y);
			}

			if (rigidbody2D.velocity.x < -maxSpeed)
			{
				rigidbody2D.velocity = new Vector2(-maxSpeed, rigidbody2D.velocity.y);
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
				anim.SetBool("Ground", false);
				rigidbody2D.AddForce(new Vector2(0f, jumpForce));
				alreadyJumped = true;
			}
		}
	}

	void WallSlide ()
	{
		if (collision.touchingWall && !collision.groundedPlus && Input.GetAxis("Horizontal") != 0)
		{
			anim.SetBool("WallSlide", true);
			minVSpeed = -5f;
		}
		else 
		{
			anim.SetBool("WallSlide", false);
			minVSpeed = -30f;
		}
	}

	void WallJump(bool jump)
	{
		if(collision.touchingWall && jump && !collision.grounded)
		{
			if (facingRight)
			{
				rigidbody2D.velocity = new Vector2(-wallPushForce, 0f);
			}
			else
			{
				rigidbody2D.velocity = new Vector2(wallPushForce, 0f);
			}
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

			Flip();
		}
	}

	void BunnyJump()
	{
		if(alreadyJumped && collision.groundedPlus && rigidbody2D.velocity.y < 0)
		{
			alreadyJumped = false;
			bunnyJumpAllowed = true;
			StartCoroutine(BunnyDelay(bunnyDelay));
		}

		if (bunnyJumpAllowed)
		{
			if (collision.grounded && Input.GetButtonUp("Jump"))
			{
				anim.SetBool("Ground", false);
				if (facingRight)
				{
					rigidbody2D.velocity = new Vector2(maxSpeed, 0f);
				}
				else
				{
					rigidbody2D.velocity = new Vector2(-maxSpeed, 0f);
				}

				rigidbody2D.AddForce(new Vector2(0f, bunnyJumpForce));
				bunnyJumpAllowed = false;
			}
		}
	}

	void JumpRecoveryF()
	{
		if(rigidbody2D.velocity.y < -recoveryLimitSpeed && collision.groundedPlus)
		{
			StartCoroutine(JumpRecovery(1f));
		}
	}

	void Sprint(bool sprint)
	{
		if(!anim.GetBool("Crouch"))
		{
			if (sprint)
			{
				maxSpeed = sprintSpeed;
			}
			else
			{
				maxSpeed = 10f;
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

	
	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag == "Ubi na dodir")
		{
			dead = true;
		}
	}

	IEnumerator BunnyDelay(float sec)
	{
		noMove = true;
		anim.SetBool("Jump Recovery", true);
		yield return new WaitForSeconds(sec);
		bunnyJumpAllowed = false;
		anim.SetBool("Jump Recovery", false);
		noMove = false;
	}

	IEnumerator JumpRecovery(float sec)
	{
		if(!dead)
		{
			noMove = true;
			anim.SetBool("Jump Recovery", true);
			yield return new WaitForSeconds(sec);
			anim.SetBool("Jump Recovery", false);
			noMove = false;
		}
	}
}