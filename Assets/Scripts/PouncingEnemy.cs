using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Darien Rogers
 * Additional Work: Arthur Clay Odom
 */

public class PouncingEnemy : MonoBehaviour
{
    Rigidbody2D rigid;

    //Makes falling down faster, giving pouncing the feeling of pouncing
    public float pounceMultiplier = 3.5f;

    //No longer used for wall detection b/c we just don't wanna work
    public Transform wallDetector;

    public Collider2D feetCollider, bodyCollider, leftWallDetector, rightWallDetector;

    bool movingLeft = true;
    bool movingRight = false;

    //Darien, why was this negative?
    public float speed = 3;

    //RaycastHit2D walls;
    RaycastHit2D player;

    //Jump height of enemy when ray encounters player
    [SerializeField]
    float jumpHeight = 0;

    //length of ray being casted from enemy
    [SerializeField]
    float playerDetectionDistance = 0;

    //If this number gets too high, we move the enemy downwards;
    private int unstickCheck = 0;

    private bool pouncing = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        wallDetection();
        playerDetection();

       fallingUpdate();

    }

    private void fallingUpdate()
    {
        //If we're pouncing, move towards the target position
        if (pouncing)
        {
            rigid.velocity += new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x) * (player.distance/(jumpHeight/Physics2D.gravity.y)+(jumpHeight/(pounceMultiplier))), 0);
        }

        if (rigid.velocity.y < 0f)
        {
            rigid.velocity -= new Vector2(0, pounceMultiplier - 1f);
        }

        if (Mathf.Abs(rigid.velocity.y) <= 0.1f && !feetCollider.IsTouchingLayers())
        {
            unstickCheck++;
        }
        else
        {
            unstickCheck = 0;
        }

        if (feetCollider.IsTouchingLayers() && pouncing)
        {
            pouncing = false;
        }

        if (unstickCheck > 10)
        {
            rigid.position += Vector2.down * Time.fixedDeltaTime;
        }
    }

    //Sends a ray from game object child of enemy to detect for players
    //If it detects a player it will cause enemy to jump
    public void playerDetection()
    {
        LayerMask playerD = LayerMask.GetMask("Player");

        if (movingLeft && !pouncing)
        {
            player = Physics2D.Raycast(wallDetector.position, Vector2.left, playerDetectionDistance, playerD);

        }
        else if (movingRight && !pouncing)
        {
            player = Physics2D.Raycast(wallDetector.position, Vector2.right, playerDetectionDistance, playerD);
        }

        if (!pouncing && (player.collider == true) && checkPounce())
        {
            pouncing = true;

            rigid.velocity = new Vector2(rigid.velocity.x, jumpHeight);
        }

    }

    public bool checkPounce()
    {
        return !(Physics2D.Linecast(new Vector2(rigid.position.x, rigid.position.y + GetComponent<CircleCollider2D>().radius), new Vector2(rigid.position.x+(-player.distance), rigid.position.y+jumpHeight), LayerMask.GetMask("Terrain")));
    }

    //Sends ray from gameobject child of enemy to detect for walls
    //If it detects a wall it will turn around and switch direction of ray
    public void wallDetection()
    {
        //LayerMask wallD = LayerMask.GetMask("Terrain");
        if (leftWallDetector.IsTouchingLayers() && (movingLeft))
        {
            movingLeft = false;
            movingRight = true;
        }
        else if (rightWallDetector.IsTouchingLayers() && (movingRight))
        {
            movingLeft = true;
            movingRight = false;
        }

        if (movingLeft)
        {
            //walls = Physics2D.Raycast(wallDetector.position, Vector2.left, bodyCollider.bounds.size.x + 1f, wallD);
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);

        }
        else if (movingRight)
        {
            //walls = Physics2D.Raycast(wallDetector.position, Vector2.right, bodyCollider.bounds.size.x + 1f, wallD);
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        }
    }
}
