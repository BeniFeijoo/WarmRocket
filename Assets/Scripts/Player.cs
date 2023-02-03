using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    SpriteRenderer mySpriteRendere;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 15f;
    [Header("Jumping")]
    [SerializeField] float initSpeedJump = 20f;
    [SerializeField] float speedJump = 20f;
    float timeChargedJumpOnSeconds = 0f;
    [SerializeField] float maxTimeChargedJumpOnSeconds = 1f;

    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite jumpSprite;
    bool isCharging = false;

    [Header("Gravity")]
    [SerializeField] float normalGravity = 12f;
    [SerializeField] float baseFallingGravity;
    [SerializeField] float fallingGravity = 17f;
    [SerializeField] float maxFallingGravity;
    bool isAlive = true;

    void Awake()
    {
        baseFallingGravity = fallingGravity;
        maxFallingGravity = baseFallingGravity + 3f;
    }
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        mySpriteRendere = GetComponent<SpriteRenderer>();
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

        if (myRigidbody.velocity.y < 0)
        {
            Debug.Log(myRigidbody.velocity.y);
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
            if (myRigidbody.velocity.y == 0)
            {
                myRigidbody.velocity = new Vector2(0, 0);
                mySpriteRendere.sprite = jumpSprite;
            }
            timeChargedJumpOnSeconds += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && isCharging || timeChargedJumpOnSeconds > maxTimeChargedJumpOnSeconds)
        {
            mySpriteRendere.sprite = idleSprite;
            isCharging = false;
            speedJump = Mathf.Round(speedJump + (speedJump * timeChargedJumpOnSeconds));
            doJump(speedJump);
            speedJump = initSpeedJump;
            timeChargedJumpOnSeconds = 0;
        }
    }
    void doJump(float jumpSpeed)
    {
        if (myRigidbody.velocity.y == 0)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void UpAndDown()
    {
        if (myRigidbody.velocity.y >= 0)
        {
            fallingGravity = baseFallingGravity;
            myRigidbody.gravityScale = normalGravity;
        }
        else if (myRigidbody.velocity.y < 0)
        {
            fallingGravity += normalGravity * Time.deltaTime;
            if (fallingGravity > maxFallingGravity)
            {
                fallingGravity = maxFallingGravity;
            }
            myRigidbody.gravityScale = fallingGravity;
        }
    }
}