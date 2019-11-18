using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    public Transform wallDetector;
    bool movingLeft = true;
    bool movingRight = false;
    float speed = -3;
    RaycastHit2D walls;
    RaycastHit2D player;
    //Jump height of enemy when ray encounters player
    [SerializeField]
    float jumpHeight = 0;
    //length of ray being casted from enemy
    [SerializeField]
    float playerDetectionDistance = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        wallDetection();
        playerDetection();


    }
    //Sends a ray from game object child of enemy to detect for players
    //If it detects a player it will cause enemy to jump
    public void playerDetection()
    {
        LayerMask playerD = LayerMask.GetMask("p");
        
        if((player.collider == true))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpHeight);
        }

        if (movingLeft)
        {
            player = Physics2D.Raycast(wallDetector.position, Vector2.left, playerDetectionDistance, playerD);

        }
        else if (movingRight)
        {
            player = Physics2D.Raycast(wallDetector.position, -Vector2.left, playerDetectionDistance, playerD);
        }




        }
    //Sends ray from gameobject child of enemy to detect for walls
    //If it detects a wall it will turn around and switch direction of ray
    public void wallDetection()
    {
        LayerMask wallD = LayerMask.GetMask("Default");
        if ((walls.collider == true) && (movingLeft))
        {
            movingLeft = false;
            movingRight = true;
        }
        else if ((walls.collider == true) && (movingRight))
        {
            movingLeft = true;
            movingRight = false;
        }

        if (movingLeft)
        {
            walls = Physics2D.Raycast(wallDetector.position, Vector2.left, 1f, wallD);
            rigid.velocity = new Vector2(speed, rigid.velocity.y);

        }
        else if (movingRight)
        {
            walls = Physics2D.Raycast(wallDetector.position, -Vector2.left, 1f, wallD);
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
        }
    }
}
