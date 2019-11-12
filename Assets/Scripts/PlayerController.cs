using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

/**
 * Author: Arthur Clay Odom
 */

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float topMoveSpeed = 100;
    public float startBoost;
    public float climbSpeed;

    public float dashSpeed;
    public float dashDrag;
    public float dashBleedOff = 10f;

    public float movingFriction = 1f;
    public float stoppingFriction = 1f;
    public float slidingFriction;

    [Range(0, 1)]
    public float lerpHorizontalMovement = 0.6f;

    [Range(1, 10)]
    public float jumpVelocity;
    //The force added to the X & Y components of the characters velocity when they kick off of a wall
    [Range(1, 7)]
    public float wallKickVerticalVelocity;
    public float wallKickHorizontalVelocity;
    [Range(0, 5)]
    public float wallKickCarryOver = 2.5f;
    // In Seconds
    //public float grabTimeLimit;

    // Causes the more snappy feeling of falling
    // Increase Gravity when Falling
    public float fallMultiplier = 2.5f;
    // Shortens a jump from a shorter input
    public float lowJumpMultiplier = 2.0f;

    private bool dash = true;
    private bool dashing = false;
    private bool grab = false;
    private bool slide = false; 

    //private Timer grabTime;

    private Vector2 movementumTracking = new Vector2(0, 0);

#pragma warning disable 0649
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
        //grabTime = new Timer(grabTimeLimit * 1000);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Housekeeping
        slide = false;

        //Stops the wind-up time to movement
        Vector2 newVelocity = new Vector2(0, rb.velocity.y);
        if (rb.velocity.x != 0)
        {
            newVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (Input.GetAxis("Horizontal") != 0)
        {
            newVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime + (Mathf.Sign(Input.GetAxis("Horizontal")) * startBoost), rb.velocity.y);
        }

        //Resets the dash
        if (feetCollider.IsTouchingLayers())
        {
            dash = true;
        }

        if (Input.GetButtonDown("Jump") && feetCollider.IsTouchingLayers())
        {
            //Debug.Log("Jump");
            //rb.velocity = new Vector2(rb.velocity.x, 0);

            newVelocity = new Vector2(newVelocity.x, 0);
            newVelocity += Vector2.up * jumpVelocity;
        }
        else if (Input.GetButtonDown("Dash") && dash && !dashing && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            Debug.Log("Dash");
            dash = false;
            dashing = true;

            rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * dashSpeed;
            //movementumTracking = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")).normalized * dashSpeed;
            rb.drag = dashDrag;
        }
        else if (Input.GetButtonDown("Jump") && leftWallKick.IsTouchingLayers())
        {
            newVelocity = new Vector2(wallKickHorizontalVelocity, newVelocity.y + wallKickVerticalVelocity);
            //movementumTracking += new Vector2(System.Math.Min(newVelocity.x, -wallKickCarryOver) + (wallKickHorizontalVelocity), 0);
        }
        else if (Input.GetButtonDown("Jump") && rightWallKick.IsTouchingLayers())
        {
            newVelocity = new Vector2(wallKickHorizontalVelocity, newVelocity.y + wallKickVerticalVelocity);
            //movementumTracking += new Vector2(-System.Math.Max(newVelocity.x, wallKickCarryOver) - (wallKickHorizontalVelocity), 0);
        }
        else if (Input.GetButton("Slide"))
        {
            newVelocity = new Vector2(rb.velocity.x - Mathf.Sign(rb.velocity.x) * slidingFriction, rb.velocity.y);
            slide = true;
        }

        //If we're falling, add fallMultiplier to gravity
        if (!grab && newVelocity.y < 0)
        {
            newVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        } //If we've stopped holding the button, increase gravity
        else if (!grab && newVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            newVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        else
        { //'Cause Unity won't do it for us
            newVelocity += Vector2.up * Physics2D.gravity.y * Time.fixedDeltaTime;
        }

        if (dashing == true)
        {
            rb.drag -= dashBleedOff;
            if (rb.drag <= 0)
            {
                dashing = false;
                rb.drag = 0;
            }
        }
        else if (!slide)
        {
            //Friction Calculations
            float xWFriction = 0f;
            if (rb.velocity.x < topMoveSpeed || rb.velocity.x > -topMoveSpeed) {
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

            if (Input.GetAxis("Horizontal") == 0 && rb.velocity.x != 0 && !(Mathf.Abs(rb.velocity.x) - Mathf.Abs(stoppingFriction) <= 0))
            {
                Debug.Log("Applied Stopping Friction: " + Time.fixedTime);
                xWFriction -= Mathf.Sign(xWFriction) * stoppingFriction;
            }

            xWFriction = Mathf.Sign(xWFriction) * Mathf.Min(Mathf.Abs(xWFriction), topMoveSpeed);

            //Final Movement Calculations
            rb.velocity = new Vector2(xWFriction, rb.velocity.y * Time.fixedDeltaTime + newVelocity.y) /*+ movementumTracking*/;
        }

      /*if ((movementumTracking.x > 0 || movementumTracking.y > 0) && movementumTracking.sqrMagnitude > 0.2f)
        {
            movementumTracking = new Vector2(Mathf.Lerp(movementumTracking.x, 0, 0.5f * Time.fixedDeltaTime), Mathf.Lerp(movementumTracking.y, 0, 0.5f * Time.fixedDeltaTime));
        }
        else if (movementumTracking.sqrMagnitude > 0.2f)
        {
            movementumTracking = new Vector2(0, 0);
        }*/
    }

    private Vector2 jump(Vector2 newVelocity)
    {
        newVelocity = new Vector2(newVelocity.x, 0);
        newVelocity += Vector2.up * jumpVelocity;

        return newVelocity;
    }

    private Vector2 wallKick(Vector2 newVelocity, bool left)
    {
        if (left)
        {
            newVelocity += new Vector2(wallKickHorizontalVelocity, wallKickVerticalVelocity);
            movementumTracking += new Vector2(System.Math.Min(newVelocity.x, -wallKickCarryOver) + (wallKickHorizontalVelocity), 0);
        }
        else
        {
            newVelocity += new Vector2(wallKickHorizontalVelocity, wallKickVerticalVelocity);
            movementumTracking += new Vector2(-System.Math.Max(newVelocity.x, wallKickCarryOver) - (wallKickHorizontalVelocity), 0);
        }

        return newVelocity;
    }

}
