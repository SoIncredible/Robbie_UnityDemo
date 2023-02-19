using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerMovement movement;
    Rigidbody2D rb;
    int groundID;
    int hangingID;
    int crouchID;
    int speedID;
    int fallID;
    private void Start()
    {
        anim = GetComponent<Animator>();
        // 获得父集的组件
        movement = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        // 获取Animator中Parameter的新方法
        groundID = Animator.StringToHash("isOnGround");
        hangingID = Animator.StringToHash("isHanging");
        crouchID = Animator.StringToHash("isCrouching");
        speedID = Animator.StringToHash("speed");
        fallID = Animator.StringToHash("verticalVelocity");

    }
    private void Update()
    {
        anim.SetFloat(speedID, Mathf.Abs(movement.xVelocity));
        // anim.SetBool("isOnGround", movement.isOnGround);
        anim.SetBool(groundID, movement.isOnGround);
        anim.SetBool(crouchID, movement.isCrouch);
        anim.SetBool(hangingID, movement.isHanging);
        anim.SetFloat(fallID, rb.velocity.y);
    }




    public void StepAudio()
    {
        AudioManager.PlayerFootstepAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.PlayerCrouchstepAudio();
    }
}
