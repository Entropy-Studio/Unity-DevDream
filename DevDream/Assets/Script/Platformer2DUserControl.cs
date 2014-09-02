using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Platformer2DUserControl : MonoBehaviour 
{
	private PlayerController character;
    private bool jump;

	void Awake()
	{
		character = GetComponent<PlayerController>();
	}

    void Update ()
    {
        // Read the jump input in Update so button presses aren't missed.
		#if CROSS_PLATFORM_INPUT
        if (CrossPlatformInput.GetButtonDown("Jump")) jump = true;
		#else
		if (Input.GetButtonDown("Jump")) jump = true;
		#endif
    }

	void FixedUpdate()
	{
		// Read the inputs.
		bool sprint = Input.GetButton("Sprint");
		bool crouch = Input.GetButton("Crouch");
		#if CROSS_PLATFORM_INPUT
		float h = CrossPlatformInput.GetAxis("Horizontal");
		#else
		float h = Input.GetAxis("Horizontal");
		#endif

		character.Action( h, crouch , jump, sprint );

        // Reset the jump input once it has been used.
	    jump = false;
	}
}