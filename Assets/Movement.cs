using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

	private Rigidbody2D character;
	[SerializeField] private SpriteRenderer characterSprite;
	[SerializeField] private Animator characterAnimator;
	[SerializeField] private float playerSpeed = 0.5f;
	[SerializeField] private float playerJumpPower = 20.0f;
	[SerializeField] private float playerMaxSpeed = 6.0f;
	private bool isGrounded = false;
	private bool walkingRight = true;

	// Start is called before the first frame update
	void Start()
	{
		character = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		ProcessInput();
	}

	void FixedUpdate()
	{
		RaycastHit2D hitLeft = Physics2D.Raycast(transform.position - new Vector3(transform.lossyScale.x / 2 - 0.2f, 0f, 0f), -Vector2.up, transform.lossyScale.y / 2, LayerMask.GetMask("World"));
		RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(transform.lossyScale.x / 2 - 0.2f, 0f, 0f), -Vector2.up, transform.lossyScale.y / 2, LayerMask.GetMask("World"));
		if (hitLeft.collider || hitRight.collider)
		{
			isGrounded = true;
			character.gravityScale = 0;
		}
		else
		{
			isGrounded = false;
			character.gravityScale = 2;
		}
	}

	void ProcessInput()
	{
		float dirX = Input.GetAxisRaw("Horizontal");
		// character.velocity = new Vector2(dirX * playerSpeed, character.velocity.y);
		character.AddForce(new Vector2(dirX * playerSpeed, 0.0f));

		if ((dirX > 0 && !walkingRight) || (dirX < 0 && walkingRight)) {
			Flip();
		}

		if (character.velocity.x > playerMaxSpeed)
		{
			character.velocity = new Vector2(playerMaxSpeed, character.velocity.y);
		}

		if (character.velocity.x < -playerMaxSpeed)
		{
			character.velocity = new Vector2(-playerMaxSpeed, character.velocity.y);
		}

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			character.velocity = new Vector2(character.velocity.x, playerJumpPower);
		}

		if (dirX != 0 && isGrounded) {
			characterAnimator.SetBool("isRunning", true);
		} else if (dirX == 0 && isGrounded) {
			characterAnimator.SetBool("isRunning", false);
		}

		characterAnimator.SetBool("isJumping", !isGrounded);

		Debug.Log(characterAnimator.GetBool("isJumping"));
	}

	void Flip()
	{
		walkingRight = !walkingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
