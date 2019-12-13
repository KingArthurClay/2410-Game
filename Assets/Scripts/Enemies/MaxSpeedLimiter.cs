using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpeedLimiter : MonoBehaviour
{

    public float maxSpeed = 100f;

    private bool running = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(startTimer());
    }

    void FixedUpdate()
    {
        if (running && rb.velocity.magnitude > maxSpeed)
        {
            Vector2 normalized = rb.velocity.normalized  * maxSpeed;

            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, normalized.x, 0.1f), Mathf.Lerp(rb.velocity.y, normalized.y, 0.1f));
        }
    }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(2f);

        running = true;
    }
}
