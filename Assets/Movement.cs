using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Movement : MonoBehaviour
{
    private Rigidbody2D character;
    private BoxCollider2D boxCollider2D;

    private bool isGrounded = false;
    private bool walkingRight = true;
    private int score = 0;
    private bool isHuge = false;
    private bool isAlive = true;

    // Store lives as static to keep them during scene restart
    private static int lives = 3;

    private float currentMovementForce = 0f;

    [SerializeField]
    private SpriteRenderer characterSprite;

    [SerializeField]
    private Animator characterAnimator;

    [SerializeField]
    private float playerSpeed = 0.5f;

    [SerializeField]
    private float playerJumpPower = 20.0f;

    [SerializeField]
    private float playerMaxSpeed = 6.0f;

    [SerializeField]
    private Tilemap worldMap;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text livesText;

    [SerializeField]
    private Image gameOverFade;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        livesText.SetText(lives.ToString());
        gameOverFade.CrossFadeAlpha(0f, 0f, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            ProcessInput();
        }
        if (transform.position.y < -10f)
        {
            if (lives == 1)
            {
                moveToGameOver();
            }
            else
            {
                lives -= 1;
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    void FixedUpdate()
    {
        character.AddForce(new Vector2(currentMovementForce * playerSpeed, 0.0f));

        RaycastHit2D hitLeft = Physics2D.Raycast(
            transform.position - new Vector3(transform.lossyScale.x / 5, 0f, 0f),
            -Vector2.up,
            transform.lossyScale.y / 2,
            LayerMask.GetMask("World")
        );
        RaycastHit2D hitRight = Physics2D.Raycast(
            transform.position + new Vector3(transform.lossyScale.x / 5, 0f, 0f),
            -Vector2.up,
            transform.lossyScale.y / 2,
            LayerMask.GetMask("World")
        );
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
        currentMovementForce = Input.GetAxisRaw("Horizontal");
        // character.velocity = new Vector2(dirX * playerSpeed, character.velocity.y);


        if (
            (currentMovementForce > 0 && !walkingRight)
            || (currentMovementForce < 0 && walkingRight)
        )
        {
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

        if (currentMovementForce != 0 && isGrounded)
        {
            characterAnimator.SetBool("isRunning", true);
        }
        else if (currentMovementForce == 0 && isGrounded)
        {
            characterAnimator.SetBool("isRunning", false);
        }

        characterAnimator.SetBool("isJumping", !isGrounded);
    }

    void Flip()
    {
        walkingRight = !walkingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "World")
        {
            Vector3 collisionDirection = (
                transform.position - collision.gameObject.transform.position
            ).normalized;

            if (collisionDirection.y <= 0.3f && collisionDirection.y >= -0.3f)
            {
                Vector3 tileLocationInWorld = collision.GetContact(0).point;
                tileLocationInWorld.y = tileLocationInWorld.y + 0.5f;
                Vector3Int tileInGrid = worldMap.WorldToCell(tileLocationInWorld);

                if (isHuge && !worldMap.GetComponent<TileMapController>().isStaticTile(tileInGrid))
                {
                    worldMap.GetComponent<TileMapController>().DestroyTileAt(tileInGrid);
                }
                else
                {
                    worldMap.GetComponent<TileMapController>().StartBounceTile(tileInGrid);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive)
        {
            return;
        }

        if (other.gameObject.tag == "Coin")
        {
            giveCoin();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Mushroom")
        {
            goHuge();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            Vector3 collisionDirection = (
                transform.position - other.gameObject.transform.position
            ).normalized;

            if ((collisionDirection.x > 0.5f || collisionDirection.x < -0.5f))
            {
                if (isHuge)
                {
                    goSmall();
                }
                else
                {
                    playDeathAnimation();
                }
            }

            if (collisionDirection.y >= 0.8f)
            {
                other.gameObject.GetComponent<EnemyController>().jumpDeath();
                character.velocity = new Vector2(character.velocity.x, 7f);
            }
        }
    }

    void goHuge()
    {
        transform.localScale = new Vector3(transform.localScale.x, 2.0f, transform.localScale.z);
        isHuge = true;
    }

    void goSmall()
    {
        transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
        isHuge = false;
    }

    void playDeathAnimation()
    {
        character.velocity = new Vector2(character.velocity.x, 10f);
        boxCollider2D.isTrigger = true;
        isAlive = false;
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.SetBool("isJumping", false);
    }

    void moveToGameOver()
    {
        gameOverFade.CrossFadeAlpha(1f, 0.5f, false);
        if (gameOverFade.canvasRenderer.GetAlpha() > 0.99f)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public void giveCoin()
    {
        score += 1;
        scoreText.SetText(score.ToString());
    }
}
