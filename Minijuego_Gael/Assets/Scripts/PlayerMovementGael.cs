using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovementGael : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private Vector2 standingOffset;
    [SerializeField] private Vector2 crouchingOffset;
    [SerializeField] private float Jumpforce = 10f;
    [SerializeField] private LayerMask Scenery;
    [SerializeField] private Transform feetpos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private Vector2 standingSize;  
    [SerializeField] private Vector2 crouchingSize;
    [SerializeField] private float moveSpeed = 5f;




    Animator animatorController;

    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;
    private float move = 0f;
    private float distanceTimer = 0f;


    public enum PlayerAnimation
    {
        crouch, walk, jump
    }

    void Start()
    {
        animatorController = GetComponentInChildren<Animator>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update()
    {
        move = 0f;
        isGrounded = Physics2D.OverlapCircle(feetpos.position, groundDistance, Scenery);

        move = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            move = moveSpeed;
        }

        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);

        if (move > 0)
        {
            distanceTimer += Time.deltaTime;

            if (distanceTimer >= 0.2f)
            {
                GameManager.Instance.ReduceDistance(1);
                distanceTimer = 0f;
            }
        }

        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
            jumpTimer = jumpTime;
            rb.linearVelocity = Vector2.up * Jumpforce;
        }

        if (Input.GetKey(KeyCode.UpArrow) && isJumping)
        {
            if (jumpTimer > 0)
            {
                rb.linearVelocity = Vector2.up * Jumpforce;
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (isGrounded && Input.GetKey(KeyCode.DownArrow))
        {
            playerCollider.size = crouchingSize;
            playerCollider.offset = crouchingOffset;
        }
        else
        {
            playerCollider.size = standingSize;
            playerCollider.offset = standingOffset;
        }
        
        UpdatePlayerAnimation();

        
    }

    void UpdatePlayerAnimation()
    {
        if (!isGrounded)
            UpdateAnimation(PlayerAnimation.jump);
        else if (Input.GetKey(KeyCode.DownArrow))
            UpdateAnimation(PlayerAnimation.crouch);
        else if (move != 0)
            UpdateAnimation(PlayerAnimation.walk);
    }

    void UpdateAnimation(PlayerAnimation nameAnimation)
    {
        switch (nameAnimation)
        {
            case PlayerAnimation.crouch:
                animatorController.SetBool("isWalking", false);
                animatorController.SetBool("isJumping", false);
                animatorController.SetBool("isCrouching", true);
                break;
            case PlayerAnimation.walk:
                animatorController.SetBool("isWalking", true);
                animatorController.SetBool("isJumping", false);
                animatorController.SetBool("isCrouching", false);
                break;
            case PlayerAnimation.jump:
                animatorController.SetBool("isWalking", false);
                animatorController.SetBool("isJumping", true);
                animatorController.SetBool("isCrouching", false);
                break;
        }



    }
}