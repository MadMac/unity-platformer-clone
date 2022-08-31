using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private bool isReadyToMove = false;
    private Rigidbody2D mushroomBody;
    private bool movingRight = true;
    private float walkingSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        BoxCollider2D playerCollider = playerObject.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, GetComponent<BoxCollider2D>(), true);

        StartCoroutine(BringUp());
    }

    // Update is called once per frame
    void Update()
    {
        if (isReadyToMove)
        {
            mushroomBody.simulated = true;
            if (movingRight)
            {
                mushroomBody.velocity = new Vector2(walkingSpeed, mushroomBody.velocity.y);
            }
            else
            {
                mushroomBody.velocity = new Vector2(-walkingSpeed, mushroomBody.velocity.y);
            }

            // Check for wall collision
            RaycastHit2D wallHit = Physics2D.Raycast(
                transform.position,
                movingRight ? Vector2.right : Vector2.left,
                0.5f,
                LayerMask.GetMask("World")
            );
            Debug.Log(movingRight);

            if (wallHit)
            {
                movingRight = !movingRight;
            }
        }
    }

    IEnumerator BringUp()
    {
        for (int i = 0; i < 10f; i++)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 0.1f,
                transform.position.z
            );
            yield return new WaitForSeconds(0.05f);
        }

        isReadyToMove = true;
    }
}
