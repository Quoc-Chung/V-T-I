using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;


    [SerializeField] private float MoveSpeed;
    [SerializeField] private float JumpForce;
    private float X_Input;
    private int faceDir = 1; /*- Hướng hiện tại */
    private bool isFacingRight = true;  /*- Nhân vật quay mặt hướng nào */


    /*- Muốn cái Header hoạt động thì đằng sau nó phải có một giá trị công khai -*/ 
    [Header("Collision info")]
    [SerializeField] private float groundcheckDistance;
    [SerializeField] private LayerMask WhatIsGround;
    private bool isGrounded;


    [Header("Dash info")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashSpeed;
    private float dashTimer;

    [SerializeField] private float dashCooldownDuration;
    private float dashCooldownTimer;
   


    private Animator anim;
    void Start()
    {
        dashCooldownTimer = dashCooldownDuration;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CheckInput();
        Movement();
        CollisionCheck();

        dashTimer -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
      
        FlipController();
        AnimatorController();
    }

    private void CheckInput()
    {
        X_Input = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }
    private void Dash()
    {
        if (dashCooldownTimer < 0)
        {
            dashCooldownTimer = dashCooldownDuration;
            dashTimer = dashDuration;
        }
    }
    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundcheckDistance, WhatIsGround);      
    }
    private void Movement()
    {
        if (dashTimer > 0)
        {                  
            rb.velocity = new Vector2(X_Input * dashSpeed,0); 
        }
        else
        {
            rb.velocity = new Vector2(X_Input * MoveSpeed, rb.velocity.y); 
        }

    }
    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }
    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("IsMoving",isMoving);
        anim.SetBool("IsGround",isGrounded);
        anim.SetBool("IsDashing", dashTimer > 0);
    }
    private void Flip()
    {
        faceDir = faceDir * (-1);
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
    private void FlipController()
    {
        if (rb.velocity.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && isFacingRight)
        {
            Flip();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y - groundcheckDistance,transform.position.z));
    }
}
