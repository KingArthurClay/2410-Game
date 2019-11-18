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

    public string target = "PlayGround";

    public void Start()
    {
        theCollider = gameObject.GetComponent<Collider2D>();
    }

    public void Update()
    {
        
        if (theCollider.IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>()) && Input.GetButton("Submit"))
        {
            LevelManager.Instance.switchScene(target);
        }

    }
}
