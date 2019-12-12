using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpittingEnemy : MonoBehaviour
{

    public float spitTime = 2f;
    public float spitVelocity = 15f;
    public float movementSpeed = 3f;

    public float accuracy = 0.01f;

    public GameObject spitPrefab;

    private Rigidbody2D rb;
    private GameObject player;
    private Rigidbody2D playerRB;

    private bool canSpit = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(new Vector2(moveCheck() * (movementSpeed * Time.fixedDeltaTime), 0));

        spit();
    }

    void spit()
    { 
        if (canSpit && playerCheck())
        {
            
            GameObject projectile = Instantiate(spitPrefab, rb.position, Quaternion.Euler(0, 0, 90));

            projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, spitVelocity));

            StartCoroutine("spitTimer");

        }
    }

    int moveCheck()
    {
        float xTarget = playerRB.position.x + playerRB.velocity.x - rb.position.x;

        if (Mathf.Abs(xTarget) < 0.01f)
        {
            return 0;
        }
        else
        {
            return (int) Mathf.Sign(xTarget);
        }
    }

    bool playerCheck()
    {

        float xClosure = (playerRB.position.x - rb.position.x) / (-playerRB.velocity.x - rb.velocity.x);
        float yClosure = (playerRB.position.y - rb.position.y) / ((-playerRB.velocity.y) + spitVelocity);

        

        if (xClosure > 0 && Mathf.Abs(xClosure % yClosure) < accuracy * xClosure)
        {
            Debug.Log(xClosure + " " + yClosure);
            Debug.Log(Mathf.Abs(xClosure % yClosure) + " " + accuracy * xClosure);

            return true;
        }

        return false;
    }

    private IEnumerator spitTimer()
    {
        canSpit = false;

        yield return new WaitForSeconds(spitTime);

        canSpit = true;
    }

}
