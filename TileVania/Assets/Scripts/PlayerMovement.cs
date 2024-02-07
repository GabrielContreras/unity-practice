using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCollider;
    float playerGravity = 1;

    [SerializeField] float playerMovementSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float playerClimbingSpeed = 5f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CapsuleCollider2D>();
        playerGravity = myRigidbody.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value) {
        if(value.isPressed && myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run(){
        Vector2 playerVelocity = new Vector2(moveInput.x * playerMovementSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        
        if(Math.Abs(playerVelocity.x) > Mathf.Epsilon) {
            myAnimator.SetBool("isRunning", true);
        } else {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) >  Mathf.Epsilon;
        if(playerHasHorizontalSpeed){
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder() {
        if(!myCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            myRigidbody.gravityScale = playerGravity;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 playerClimbingVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * playerClimbingSpeed);   
        myRigidbody.velocity = playerClimbingVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
}
