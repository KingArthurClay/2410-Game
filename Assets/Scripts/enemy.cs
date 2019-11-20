using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Darien Rogers
 * Additional Work: Arthur Clay Odom
 */

public class enemy : MonoBehaviour
{
    Rigidbody2D rigid;

    //No longer used for wall detection b/c we just don't wanna work
    public Transform wallDetector;

    public Collider2D feetCollider, bodyCollider, leftWallDetector, rightWallDetector;

    bool movingLeft = true;
    bool movingRight = false;

    //Darien, why was this negative?
    public float speed = 3;

    RaycastHit2D walls;
    RaycastHit2D player;

    //Jump height of enemy when ray encounters player
    [SerializeField]
    float jumpHeight = 0;

    //length of ray being casted from enemy
    [SerializeField]
    float playerDetectionDistance = 0;

    //If this number gets too high, we move the enemhy downwards;
    private int unstickCheck = 0;

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

        unstickingCheck();

    }

    private void unstickingCheck()
    {
        if (Mathf.Abs(rigid.velocity.y) <= 0.1f && !feetCollider.IsTouchingLayers())
        {
            unstickCheck++;
        }
        else
        {
            unstickCheck = 0;
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

        if (movingLeft)
        {
            player = Physics2D.Raycast(wallDetector.position, Vector2.left, playerDetectionDistance, playerD);

        }
        else if (movingRight)
        {
            player = Physics2D.Raycast(wallDetector.position, Vector2.right, playerDetectionDistance, playerD);
        }

        if ((player.collider == true))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpHeight);
        }

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
