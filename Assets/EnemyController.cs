using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D enemyCharacter;
    private BoxCollider2D boxCollider2D;

    private bool walkingRight = true;

    [SerializeField]
    private float walkingSpeed = 2.0f;

    [SerializeField]
    private BoxCollider2D playerCollider;

    void Start()
    {
        // Set initial walking direction based on direction the enemy is directed initially
        walkingRight = transform.localScale.x == 1;
        boxCollider2D = GetComponent<BoxCollider2D>();

        enemyCharacter = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(playerCollider, boxCollider2D, true);
    }

    void Update()
    {
        moveEnemy();
    }

    void moveEnemy()
    {
        if (walkingRight)
        {
            enemyCharacter.velocity = new Vector2(walkingSpeed, enemyCharacter.velocity.y);
        }
        else
        {
            enemyCharacter.velocity = new Vector2(-walkingSpeed, enemyCharacter.velocity.y);
        }

        // Check for wall collision
        RaycastHit2D wallHit = Physics2D.Raycast(
            transform.position,
            walkingRight ? Vector2.right : Vector2.left,
            0.5f,
            LayerMask.GetMask("World")
        );

        if (wallHit)
        {
            Flip();
        }
    }

    void Flip()
    {
        walkingRight = !walkingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
