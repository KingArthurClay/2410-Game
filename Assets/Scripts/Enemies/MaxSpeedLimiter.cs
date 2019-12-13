using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpeedLimiter : MonoBehaviour
{

    public float maxSpeed = 100f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            Vector2 normalized = rb.velocity.normalized  * maxSpeed;

            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, normalized.x, 0.1f), Mathf.Lerp(rb.velocity.y, normalized.y, 0.1f));
        }
    }
}
