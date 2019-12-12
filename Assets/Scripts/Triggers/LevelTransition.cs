using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author:  Arthur Clay Odom
 */

public class LevelTransition : MonoBehaviour
{

    private Collider2D theCollider;
    private Animator anim;
    private fadeScript fade;

    public string target = "PlayGround";

    public void Start()
    {
        theCollider = gameObject.GetComponent<Collider2D>();

        anim = GameObject.Find("UI").GetComponentInChildren<Animator>();
        fade = GameObject.Find("UI").GetComponentInChildren<fadeScript>();
    }

    public void Update()
    {
        
        if (theCollider.IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().getCollider()) && Input.GetButton("Submit"))
        {
            anim.SetTrigger("fadeOut");

            fade.currentTarget = target;
        }

    }
}
