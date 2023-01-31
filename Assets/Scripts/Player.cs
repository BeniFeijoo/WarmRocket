using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float initSpeedJump = 5f;
    [SerializeField] float speedJump = 5f;
    float timeChargedJumpOnSeconds = 0f;
    float maxTimeChargedJumpOnSeconds = 2f;
    bool isCharging = false;
    bool isAlive = true;

    float normalGravity = 1f;
    float fallingGravity = 2f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isAlive)
        {
            Run();
            Jump();
            UpAndDown();
        }
    }

    void Run()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
        }
        if (isCharging)
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Debug.Log("Freeze");
                myRigidbody.velocity = new Vector2(0, 0);
            }
            timeChargedJumpOnSeconds += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && isCharging || timeChargedJumpOnSeconds > maxTimeChargedJumpOnSeconds)
        {
            isCharging = false;
            speedJump = Mathf.Round(speedJump + (speedJump * timeChargedJumpOnSeconds));
            doJump(speedJump);
            speedJump = initSpeedJump;
            timeChargedJumpOnSeconds = 0;
        }
    }
    void doJump(float jumpSpeed)
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void UpAndDown()
    {
        if (myRigidbody.velocity.y >= 0)
        {
            myRigidbody.gravityScale = normalGravity;
        }
        else if (myRigidbody.velocity.y < 0)
        {
            fallingGravity += normalGravity * Time.deltaTime;
            if (fallingGravity > 5)
            {
                fallingGravity = 5;
            }
            myRigidbody.gravityScale = fallingGravity;
        }
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            fallingGravity = 2f;
        }
    }
}