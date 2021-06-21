using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float playerSpeed;
    public float playerAcceleration;
    public float playerDeceleration;
    public float maxSpeed;
    public float jumpVelocity;
    public float maxJumpHeight;
    public float gravityValueInitial;
    public float gravityValueFinal;

    private RaycastHit2D hitInnerLeft;
    private RaycastHit2D hitInnerRight;
    private RaycastHit2D hitOuterLeft;
    private RaycastHit2D hitOuterRight;
    private RaycastHit2D hitTopLeft;
    private RaycastHit2D hitTopRight;
    public float distanceUpwards;
    public float nudgeAmount;
    public float innerSize;
    public float outerSize;
    private float translateAmount;

    private RaycastHit2D hitInnerBottomR;
    private RaycastHit2D hitInnerTopR;
    private RaycastHit2D hitOuterBottomR;
    private RaycastHit2D hitOuterTopR;
    private RaycastHit2D hitGapTopR;
    private RaycastHit2D hitGapBottomR;

    private RaycastHit2D hitInnerBottomL;
    private RaycastHit2D hitInnerTopL;
    private RaycastHit2D hitOuterBottomL;
    private RaycastHit2D hitOuterTopL;
    private RaycastHit2D hitGapTopL;
    private RaycastHit2D hitGapBottomL;

    private float jumpTimeCounter;
    public float jumpTime;

    private float hangTimeCounter;
    public float hangTime;

    private float jumpBufferAmount;
    public float jumpBufferLength;

    private float moveInput;
    private bool jumpInput;
    private bool jumpInputHold;
    private bool allowJump;
    private bool isJumping;

    public float groundedSkin = 0.05f;
    public LayerMask groundMask;
    private bool isGrounded;
    Vector2 playerSize;
    Vector2 boxSize;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<BoxCollider2D>().size;
        boxSize = new Vector2(playerSize.x - 0.1f, groundedSkin);
    }

    void FixedUpdate() {
        CornerCollisionMove();
        MoveX();
        MoveY();
    }

    void Update() {
        CornerCollisionDetection();
        PlayerInputDetection();
        GroundDetection();
    }

    void MoveX() {
        if(rb.velocity.x > maxSpeed) {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        } else if(rb.velocity.x < -maxSpeed) {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
        if(Mathf.Abs(rb.velocity.x) <= maxSpeed) {
            if(moveInput != 0) {
                rb.velocity += new Vector2(moveInput * playerSpeed * playerAcceleration * Time.fixedDeltaTime, 0);
            } else {
                if(rb.velocity.x < 0) {
                    rb.velocity += new Vector2(playerSpeed * playerDeceleration * Time.fixedDeltaTime, 0);
                    if(rb.velocity.x > 0) {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                } else if(rb.velocity.x > 0) {
                    rb.velocity -= new Vector2(playerSpeed * playerDeceleration * Time.fixedDeltaTime, 0);
                    if(rb.velocity.x < 0) {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
            }
        }
    }

    void MoveY() {
        if(isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        
        if(allowJump) {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            isJumping = true;

            allowJump = false;
            isGrounded = false;
        }

        if(isJumping && jumpInputHold && !isGrounded) {
            if(jumpTimeCounter > 0) {
                rb.velocity += new Vector2(0, jumpVelocity);
                jumpTimeCounter -= Time.fixedDeltaTime;
            } else {
                isJumping = false;
            }
        }

        if((!jumpInputHold || rb.velocity.y < 0) && !isGrounded) {
            isJumping = false;
            rb.gravityScale = gravityValueFinal;
        } else {
            rb.gravityScale = gravityValueInitial;
        }
    }

    void PlayerInputDetection() {
        moveInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetKeyDown(KeyCode.I);
        jumpInputHold = Input.GetKey(KeyCode.I);
    }

    void GroundDetection() {
        if(!allowJump) {
            Vector2 boxCenter = (Vector2) transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
            isGrounded = (Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundMask) != null);
        }

        if(isGrounded) {
            hangTimeCounter = hangTime;
        } else {
            hangTimeCounter -= Time.deltaTime;
        }

        if(jumpInput) {
            jumpBufferAmount = jumpBufferLength;
        } else {
            jumpBufferAmount -= Time.deltaTime;
        }
        
        if(jumpBufferAmount >= 0 && hangTimeCounter > 0) {
            allowJump = true;
            jumpTimeCounter = jumpTime;
            jumpBufferAmount = 0;
        }
    }

    void CornerCollisionDetection() {
        // corner collision top
        hitInnerLeft = Physics2D.Raycast(transform.position - new Vector3(innerSize, 0, 0), Vector2.up, distanceUpwards, groundMask);
        hitInnerRight = Physics2D.Raycast(transform.position + new Vector3(innerSize, 0, 0), Vector2.up, distanceUpwards, groundMask);
        hitOuterLeft = Physics2D.Raycast(transform.position - new Vector3(outerSize, 0, 0), Vector2.up, distanceUpwards, groundMask);
        hitOuterRight = Physics2D.Raycast(transform.position + new Vector3(outerSize, 0, 0), Vector2.up, distanceUpwards, groundMask);
        hitTopLeft = Physics2D.Raycast(transform.position + new Vector3(-innerSize, distanceUpwards, 0), Vector2.left, outerSize - innerSize, groundMask);
        hitTopRight = Physics2D.Raycast(transform.position + new Vector3(innerSize, distanceUpwards, 0), Vector2.right, outerSize - innerSize, groundMask);

        // corner collision left
        hitInnerBottomL = Physics2D.Raycast(transform.position - new Vector3(0, innerSize, 0), Vector2.left, distanceUpwards, groundMask);
        hitInnerTopL = Physics2D.Raycast(transform.position + new Vector3(0, innerSize, 0), Vector2.left, distanceUpwards, groundMask);
        hitOuterBottomL = Physics2D.Raycast(transform.position - new Vector3(0, outerSize, 0), Vector2.left, distanceUpwards, groundMask);
        hitOuterTopL = Physics2D.Raycast(transform.position + new Vector3(0, outerSize, 0), Vector2.left, distanceUpwards, groundMask);
        hitGapTopL = Physics2D.Raycast(transform.position + new Vector3(-distanceUpwards, innerSize, 0), Vector2.up, outerSize - innerSize, groundMask);
        hitGapBottomL = Physics2D.Raycast(transform.position + new Vector3(-distanceUpwards, -innerSize, 0), Vector2.down, outerSize - innerSize, groundMask);

        // corner collision right
        hitInnerBottomR = Physics2D.Raycast(transform.position - new Vector3(0, innerSize, 0), Vector2.right, distanceUpwards, groundMask);
        hitInnerTopR = Physics2D.Raycast(transform.position + new Vector3(0, innerSize, 0), Vector2.right, distanceUpwards, groundMask);
        hitOuterBottomR = Physics2D.Raycast(transform.position - new Vector3(0, outerSize, 0), Vector2.right, distanceUpwards, groundMask);
        hitOuterTopR = Physics2D.Raycast(transform.position + new Vector3(0, outerSize, 0), Vector2.right, distanceUpwards, groundMask);
        hitGapTopR = Physics2D.Raycast(transform.position + new Vector3(distanceUpwards, innerSize, 0), Vector2.up, outerSize - innerSize, groundMask);
        hitGapBottomR = Physics2D.Raycast(transform.position + new Vector3(distanceUpwards, -innerSize, 0), Vector2.down, outerSize - innerSize, groundMask);
    }

    void CornerCollisionMove() {
        // corner collision top
        if(hitOuterLeft.collider != null && hitInnerLeft.collider == null && jumpInputHold) {
            translateAmount = Mathf.Abs(hitTopLeft.point.x - hitOuterLeft.point.x + 0.01f);
            transform.Translate(translateAmount, 0, 0);
        }
        if(hitOuterRight.collider != null && hitInnerRight.collider == null && jumpInputHold) {
            translateAmount = Mathf.Abs(hitOuterRight.point.x - hitTopRight.point.x + 0.01f);
            transform.Translate(-translateAmount, 0, 0);
        }
        
        // corner collision left
        if(hitOuterTopL.collider != null && hitInnerTopL.collider == null && moveInput < 0) {
            translateAmount = Mathf.Abs(hitOuterTopL.point.y - hitGapTopL.point.y + 0.05f);
            transform.Translate(0, -translateAmount, 0);
        }
        if(hitOuterBottomL.collider != null && hitInnerBottomL.collider == null && moveInput < 0) {
            translateAmount = Mathf.Abs(hitGapBottomL.point.y - hitOuterBottomL.point.y + 0.05f);
            transform.Translate(0, translateAmount, 0);
            Debug.Log("outer bottom left");
        }

        // corner collision right
        if(hitOuterTopR.collider != null && hitInnerTopR.collider == null && moveInput > 0) {
            translateAmount = Mathf.Abs(hitOuterTopR.point.y - hitGapTopR.point.y + 0.05f);
            transform.Translate(0, -translateAmount, 0);
        }
        if(hitOuterBottomR.collider != null && hitInnerBottomR.collider == null && moveInput > 0) {
            translateAmount = Mathf.Abs(hitGapBottomR.point.y - hitOuterBottomR.point.y + 0.05f);
            transform.Translate(0, translateAmount, 0);
        }
    }
}
