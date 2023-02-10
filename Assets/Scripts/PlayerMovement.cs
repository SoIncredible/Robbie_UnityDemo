using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Vector2 collStandOffset;
    private Vector2 collStandSize;
    private Vector2 collCrouchOffset;
    private Vector2 collCrouchSize;
    [Header("移动参数")]
    public float speed = 8f;
    public float crounchSpeedDivisor = 3f;

    
    [Header("状态参数")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    [Header("跳跃参数")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;

    [Header("检测环境")]
    public LayerMask groundLayer;

    float jumpTime;
    float xVelocity;
    [Header("按键设置")]
    public bool jumpPressed;
    public bool jumpHeld;
    public bool crouchHeld;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        collStandOffset = new Vector2(coll.offset.x, coll.offset.y);
        collStandSize = new Vector2(coll.size.x, coll.size.y);
        collCrouchOffset = new Vector2(collStandOffset.x, collStandOffset.y / 2f);
        collCrouchSize = new Vector2(collStandSize.x, collStandSize.y / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
        
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
    }
    private void FixedUpdate()
    {
        if (!Input.GetButton("Jump"))
        {
            jumpPressed = false;
        }
        PhysicsCheck();
        GroundMovement();
        MidAriMovement();
        FaceDirection();
    }

    void PhysicsCheck()
    {
        if (coll.IsTouchingLayers(groundLayer))
            isOnGround = true;
        else isOnGround = false;
    }


    void GroundMovement()
    {

        if (crouchHeld && !isCrouch && isOnGround)
        {
            Crouch();
        }else if (!crouchHeld && isCrouch)
        {
            StandUp();
        }else if(!isOnGround && isCrouch)
        {
            StandUp();
        }

        xVelocity = Input.GetAxis("Horizontal");
        if (isCrouch)
        {
            xVelocity /= crounchSpeedDivisor;
        }
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
    }


    void MidAriMovement()
    {
        if(jumpPressed && isOnGround)
        {

            if(isCrouch && isOnGround)
            {
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }

            isOnGround = false;
            isJump = true;

            jumpTime = Time.time + jumpHoldDuration;

            //突然产生一个力
            rb.AddForce(new Vector2(0f,jumpForce),ForceMode2D.Impulse);
            jumpPressed = false;
        }else if (isJump)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if (jumpTime < Time.time)
                isJump = false;
            jumpPressed = false;
        }
    }



    void FaceDirection()
    {
        if(xVelocity > 0)
        {
            rb.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }else if(xVelocity < 0)
        {
            rb.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void Crouch()
    {
        isCrouch = true;
        coll.size = collCrouchSize;
        coll.offset = collCrouchOffset;
    }
    void StandUp()
    {
        isCrouch = false;
        coll.size = collStandSize;
        coll.offset = collStandOffset;
    }
}
