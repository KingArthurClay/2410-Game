using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncy : MonoBehaviour
{

    private Rigidbody2D rb;

    public PhysicsMaterial2D bouncyMat, normal;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            rb.sharedMaterial = bouncyMat;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.sharedMaterial = normal;
        }
    }

}
