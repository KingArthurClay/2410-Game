using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyDeathHandler : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D feetCollider;

    public PhysicsMaterial2D player, bouncy;

    private void Start()
    {
        anim = GameObject.Find("UI").GetComponentInChildren<Animator>();
        feetCollider = GetComponentInParent<PouncingEnemy>().feetCollider;
        rb = GetComponentInParent<Rigidbody2D>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.Play("playerDeath");
        }
    }

    public void Update()
    {
        if (feetCollider.IsTouchingLayers())
        {
            rb.sharedMaterial = player;
        }
        else
        {
            rb.sharedMaterial = bouncy;
        }
    }
}
