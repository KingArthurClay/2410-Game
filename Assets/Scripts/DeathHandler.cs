using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{

    private Animator anim;

    private void Start()
    {
        anim = GameObject.Find("UI").GetComponentInChildren<Animator>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.Play("playerDeath");

            SpawnRespawnHandler.Instance.respawnPlayer();
        }
    }

    /*public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SpawnRespawnHandler.Instance.respawnPlayer();
        }
    }*/
}
