using UnityEngine;
using System.Collections;

public class PlayerController : RexJumpElement
{
	public float mSpeed;   
	public float jForce;
	public float jTime;
    public float groundCheckRadius;
	public bool onGround;
	public LayerMask whatIsGround;
	public Transform groundChecker;

    public Animator myAnimator { get; set; }
    public Rigidbody2D playerRigidbody2D { get; set; }
    public bool playerPlays { get; set; }
    public bool stoppedJumping { get; set; }
    public float jTimeCounter { get; set; }
    public float mSpeedStore { get; set; }
	public float speedMilestoneCount { get; set; }
	public float speedMilestoneCountStore { get; set; }
       
	void Start () 
	{
		PlayerControllerStart();
	}	
    
	void Update ()
	{
		SpeedDifficulty();
        PlayerVelocity();
        PlayerOnGround();
        PlayerControls();
	}

	public void PlayerControllerStart ()
	{

		playerRigidbody2D = app.view.player.GetComponent<Rigidbody2D>();

		// playerCollider = GetComponent<Collider2D>();

		myAnimator = app.view.player.GetComponent<Animator>();
		myAnimator.Play("Run00");

		jTimeCounter = jTime;

		speedMilestoneCount = app.model.speedIncreaseMilestone;

		// Store mSpeed and sPeedMilestoneCount

		mSpeedStore = mSpeed;
		// Debug.Log("mSpeed Store " + mSpeedStore);

		speedMilestoneCountStore = speedMilestoneCount;
		app.model.speedIncreaseMilestoneStore = app.model.speedIncreaseMilestone;

		stoppedJumping = true;
	}

	public void PlayerOnGround ()
	{
		onGround = Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, whatIsGround);
	}

	public void PlayerVelocity ()
	{
		playerRigidbody2D.velocity = new Vector2 (playerRigidbody2D.velocity.x, playerRigidbody2D.velocity.y);
		app.view.endlessScroller.transform.Translate (Vector3.left * mSpeed * Time.deltaTime);
	}

	public void SpeedDifficulty ()
	{
		if (app.model.scoreCount > speedMilestoneCount)
		{
			speedMilestoneCount += app.model.speedIncreaseMilestone;

			app.model.speedIncreaseMilestone = speedMilestoneCount * app.model.speedMultiplier;

			mSpeed *= app.model.speedMultiplier;
		}
	}

	public void PlayerControls ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			if (onGround)
			{
				playerRigidbody2D.velocity = new Vector2 (playerRigidbody2D.velocity.x, jForce);
				jTimeCounter = jTime;
				stoppedJumping = false;
			}

		}

		// Long Jump Setttings

		if (Input.GetMouseButton (0) && !stoppedJumping)
		{
			if (jTimeCounter > 0)
			{
				playerRigidbody2D.velocity = new Vector2 (playerRigidbody2D.velocity.x, jForce);
				jTimeCounter -= Time.deltaTime;
			}
		}

		if (Input.GetMouseButtonUp (0))
		{
			jTimeCounter = 0;
			stoppedJumping = true;
		}

		if (onGround)
		{
			jTimeCounter = jTime;
		}

		myAnimator.SetFloat ("AnimSpeed", mSpeed);
		myAnimator.SetBool ("PlayerGrounded", onGround);
	}

}
