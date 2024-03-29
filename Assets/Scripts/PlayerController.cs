﻿using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

/**
 * Author: Arthur Clay Odom
 */

public class PlayerController : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed = 5;
    public float topMoveSpeed = 100;
    public float startBoost;
    public float boostPoint = 1;

    [Header("Dashing")]
    public float dashSpeed;
    public float dashDrag;
    public float dashBleedOff = 10f;

    [Header("Friction")]
    public float movingFriction = 1f;
    public float stoppingFriction = 1f;
    public float slidingFriction = 0.45f;

    [Header("'Fun' Stuff")]
    [Range(0, 1)]
    public float lerpHorizontalMovement = 0.6f;

    [Range(1, 10)]
    public float jumpVelocity;
    //The force added to the X & Y components of the characters velocity when they kick off of a wall
    [Range(1, 7)]
    public float wallKickVerticalVelocity;
    public float wallKickHorizontalVelocity;
    public float jumpDelay;
    // In Seconds
    //public float grabTimeLimit;

    [Header("Gravity")]
    // Causes the more snappy feeling of falling
    // Increase Gravity when Falling
    public float fallMultiplier = 2.5f;
    // Shortens a jump from a shorter input
    public float lowJumpMultiplier = 2.0f;
    public float wallSlideMultiplier = .24f;

    public bool dash = true;

    private bool dashing = false;
    private bool wallSliding = false;
    private bool slide = false;
    private bool canJump = true;

    private Animator anim;

    private SpriteRenderer sprite;

    //private Timer grabTime;

#pragma warning disable 0649
    [SerializeField]
    Collider2D normalCollider;
    [SerializeField]
    CapsuleCollider2D slidingCollider;
    [SerializeField]
    CapsuleCollider2D feetCollider;
    [SerializeField]
    CapsuleCollider2D leftWallKick;
    [SerializeField]
    CapsuleCollider2D rightWallKick;
    [SerializeField]
    CapsuleCollider2D leftGrab;
    [SerializeField]
    CapsuleCollider2D rightGrab;
#pragma warning disable 0649

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        sprite = GetComponent<SpriteRenderer>();
        //grabTime = new Timer(grabTimeLimit * 1000);

        //Force the Respawn system to function
        SpawnRespawnHandler.Instance.findRespawn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool wallJump = false;

        if (rb.velocity.x < 0)
        {
            sprite.flipX = true;
        }
        else if (rb.velocity.x > 0)
        {
            sprite.flipX = false;
        }

        //Sliding Housekeeping
        if (slide && !((Input.GetButton("Slide") || Input.GetAxis("Vertical") < 0))) {
            slide = false;

            normalCollider.enabled = true;

            anim.Play("PlayerIdle");
        }

        //Stops the wind-up time to movement
        Vector2 newVelocity = new Vector2(0, rb.velocity.y);
        if (!slide && !(Mathf.Abs(rb.velocity.x) < boostPoint))
        {
            newVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (!slide && Input.GetAxis("Horizontal") != 0)
        {
            newVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime + (Mathf.Sign(Input.GetAxis("Horizontal")) * startBoost), rb.velocity.y);
        }

        if (!slide  && rb.velocity.magnitude == 0)
        {
            anim.Play("PlayerIdle");
        }
        else if (!slide && rb.velocity.y == 0)
        {
            anim.Play("PlayerRunNew");
        }

        //Resets the dash
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Terrain")))
        {
            dash = true;
        }

        if (canJump && Input.GetButtonDown("Jump") && feetCollider.IsTouchingLayers(LayerMask.GetMask("Terrain")))
        {   //Jumping

            anim.Play("PlayerJump");

            newVelocity = new Vector2(newVelocity.x, 0);
            newVelocity += Vector2.up * jumpVelocity;
        }
        else if (Input.GetButtonDown("Dash") && dash && !dashing && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {   //Dashing
            dash = false;
            dashing = true;

            anim.Play("PlayerDash");

            newVelocity += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * dashSpeed;
            rb.drag = dashDrag;
        }
        else if (canJump && Input.GetButtonDown("Jump") && leftWallKick.IsTouchingLayers(LayerMask.GetMask("Terrain")))
        {   //Wall Kicks to the Right
            Debug.Log(newVelocity);

            if (wallSliding)
            {
                //Debug.Log("Jumped off to the right");
                newVelocity = new Vector2(wallKickHorizontalVelocity, wallKickVerticalVelocity);
            }
            else
            {
                newVelocity = new Vector2(wallKickHorizontalVelocity, newVelocity.y + wallKickVerticalVelocity);
            }

            Debug.Log(newVelocity);
            wallSliding = false;
            anim.Play("PlayerJump");

            wallJump = true;
            
            StartCoroutine(JumpDelay());
        }
        else if (canJump && Input.GetButtonDown("Jump") && rightWallKick.IsTouchingLayers(LayerMask.GetMask("Terrain")))
        {   //Wall Kicks to the Left
            
            if (wallSliding)
            {
                //Debug.Log("Jumped off to the left");
                newVelocity = new Vector2(-wallKickHorizontalVelocity, wallKickVerticalVelocity);
                
            }
            else
            {
                newVelocity = new Vector2(-wallKickHorizontalVelocity, newVelocity.y + wallKickVerticalVelocity);
            }

            wallSliding = false;
            anim.Play("PlayerJump");
            wallJump = true;
            StartCoroutine(JumpDelay());
        }
        else if ((Input.GetButton("Slide") || Input.GetAxis("Vertical") < 0) && Mathf.Abs(rb.velocity.x) > 1)
        {   //Sliding
            newVelocity = new Vector2(0, 0);

            if (wallSliding)
            {
                //Debug.Log("Slid out of wallSliding");
                wallSliding = false;
            }

            //Slide disables normal input
            if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Terrain")))
            {
                newVelocity = new Vector2(-Mathf.Sign(rb.velocity.x) * slidingFriction, rb.velocity.y);
            }
            else
            {
                newVelocity = new Vector2(0, rb.velocity.y);
            }
            
            normalCollider.enabled = false;

            slide = true;

            anim.Play("PlayerSlide");
        }

        if (!slide && leftGrab.IsTouchingLayers(LayerMask.GetMask("Terrain")) || rightGrab.IsTouchingLayers(LayerMask.GetMask("Terrain"))) //No wallSlide if the player is sliding
        {   //Wall Sliding

            //We Only want to slide on a wall if the player pushes against it
            if (leftGrab.IsTouchingLayers(LayerMask.GetMask("Terrain")) && Input.GetAxis("Horizontal") < 0)
            {
                anim.Play("PlayerClimb");
                newVelocity.y = 0;
                newVelocity.x = 0;
                wallSliding = true;

                sprite.flipX = true;
            }
            else if (rightGrab.IsTouchingLayers(LayerMask.GetMask("Terrain")) && Input.GetAxis("Horizontal") > 0)
            {
                anim.Play("PlayerClimb");
                newVelocity.y = 0;
                newVelocity.x = 0;
                wallSliding = true;

                sprite.flipX = false;
            }
        }

        if (wallJump) Debug.Log(newVelocity);
        //If we're falling, add fallMultiplier to gravity
        if (!wallSliding && newVelocity.y < 0)
        {
            newVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;

            anim.Play("PlayerFall");
        } //If we've stopped holding the button, increase gravity
        else if (!wallSliding && newVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            newVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (wallSliding)
        {   //If we'er wallSliding, act like it
            //anim.Play("PlayerWallSlide");

            newVelocity += Vector2.up * Physics2D.gravity.y * wallSlideMultiplier * Time.fixedDeltaTime;
            
        }
        else
        { //'Cause Unity won't do it for us
            newVelocity += Vector2.up * Physics2D.gravity.y * Time.fixedDeltaTime;
        }

        if (dashing == true) //Handles Dashing Drag and Drab bleed-off
        {
            rb.drag -= dashBleedOff;
            if (rb.drag <= 0)
            {
                dashing = false;
                rb.drag = 0;
            }

            rb.velocity = newVelocity;
        }
        else if (slide) //If we're sliding, we've done this already
        {
            rb.velocity = new Vector2(rb.velocity.x + newVelocity.x, rb.velocity.y * Time.fixedDeltaTime + newVelocity.y);
        }
        else if (!slide && Input.GetAxis("Horizontal") != 0) //Moving Friction Calculations
        {
            //Friction Calculations
            float xWFriction = 0f;
            if (wallJump || rb.velocity.x <= topMoveSpeed || rb.velocity.x >= -topMoveSpeed) { //If we're not going to exceed our top move speed
                if (rb.velocity.x + newVelocity.x > 0)
                {
                    if (rb.velocity.x + newVelocity.x - movingFriction > 0)
                    {
                        xWFriction = rb.velocity.x + newVelocity.x - movingFriction;
                    }
                }
                else if (rb.velocity.x + newVelocity.x < 0)
                {
                    if (rb.velocity.x + newVelocity.x + movingFriction < 0)
                    {
                        xWFriction = rb.velocity.x + newVelocity.x + movingFriction;
                    }
                }
            }
            else //Just cut the player's input from the friction
            {
                if (rb.velocity.x > 0)
                {
                    if (rb.velocity.x - movingFriction > 0 && !(rb.velocity.x <= movingFriction))
                    {
                        xWFriction = rb.velocity.x - movingFriction;
                    }
                }
                else if (rb.velocity.x + newVelocity.x < 0)
                {
                    if (rb.velocity.x + movingFriction < 0 && !(rb.velocity.x >= -movingFriction))
                    {
                        xWFriction = rb.velocity.x + movingFriction;
                    }
                }
            }

            xWFriction = Mathf.Sign(xWFriction) * Mathf.Min(Mathf.Abs(xWFriction), topMoveSpeed);

            //Final Movement Calculations
            rb.velocity = new Vector2(xWFriction, rb.velocity.y * Time.fixedDeltaTime + newVelocity.y);
        }
        else if (!slide) //Stopped Friction Calculations
        {
            float xWFriction = 0;

            if (Input.GetAxis("Horizontal") == 0 && rb.velocity.x != 0 && !(Mathf.Abs(rb.velocity.x) - Mathf.Abs(stoppingFriction) <= 0))
            {
                xWFriction = rb.velocity.x; 

              ////Debug.Log("Applied Stopping Friction: " + Time.fixedTime);
                xWFriction -= Mathf.Sign(xWFriction) * stoppingFriction;
            }
            else if (wallJump)
            {
                xWFriction = newVelocity.x;
            }

            xWFriction = Mathf.Sign(xWFriction) * Mathf.Min(Mathf.Abs(xWFriction), topMoveSpeed);

            //Final Movement Calculations
            rb.velocity = new Vector2(xWFriction, rb.velocity.y * Time.fixedDeltaTime + newVelocity.y);
            if (wallJump) Debug.Log("Stopped "+rb.velocity);
        }
        
        //Housekeeping check to make sure wallsliding doesn't happen forever
        if (wallSliding && !leftGrab.IsTouchingLayers(LayerMask.GetMask("Terrain")) && !rightGrab.IsTouchingLayers(LayerMask.GetMask("Terrain")))
        {
            //Debug.Log("Not Wallsliding!");
            wallSliding = false;
        }

    }

    IEnumerator JumpDelay()
    {
        canJump = false;

        yield return new WaitForSeconds(jumpDelay);

        canJump = true;
    }

    public Collider2D getCollider()
    {
        return normalCollider;
    }
    
}
