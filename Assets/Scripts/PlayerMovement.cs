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
    [Header("�ƶ�����")]
    public float speed = 8f;
    public float crounchSpeedDivisor = 3f;

    
    [Header("״̬����")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlocked;
    public bool isHanging;

    [Header("��Ծ����")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;

    public float hangJumpForce = 15f;

    [Header("��⻷��")]
    public LayerMask groundLayer;
    public float footOffset = 0.4f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;
    float playerHeight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;



    float jumpTime;
    float xVelocity;
    [Header("��������")]
    public bool jumpPressed;
    public bool jumpHeld;
    public bool crouchHeld;
    //���ΰ���
    public bool crouchPressed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();


        playerHeight = coll.size.y;

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
        if (Input.GetButtonDown("Crouch"))
        {
            crouchPressed = true;
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
        /*
        Vector2 pos = transform.position;
        Vector2 offset = new Vector2(-footOffset, 0f);
        RaycastHit2D lef;
        */
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        if(leftCheck || rightCheck)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);
        if (headCheck)
        {
            isHeadBlocked = true;
        }
        else
        {
            isHeadBlocked = false;
        }
        
        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);


        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight),grabDir,grabDistance,groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);
    
        if(!isOnGround && rb.velocity.y < 0 && ledgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;


            pos.x += (wallCheck.distance - 0.05f) * direction;
            pos.y -= ledgeCheck.distance;
            transform.position = new Vector2(pos.x, pos.y);
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        } 
    
    }


    void GroundMovement()
    {

        if (isHanging)
            return;

        if (crouchHeld && !isCrouch && isOnGround)
        {
            Crouch();
        }else if (!crouchHeld && isCrouch && !isHeadBlocked)
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

        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangJumpForce);
                isHanging = false;
            }
            if (crouchPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }


        if(jumpPressed && isOnGround && !isHeadBlocked)
        {

            if(isCrouch && isOnGround)
            {
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
                crouchPressed = false;
            }

            isOnGround = false;
            isJump = true;

            jumpTime = Time.time + jumpHoldDuration;

            //ͻȻ����һ����
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

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDiraction * length, color);
        return hit;
    }
}
