using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author:  Arthur Clay Odom
 */

public class LevelTransition : MonoBehaviour
{

    private Collider2D collider;

    public string target = "PlayGround";

    public void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();
    }

    public void Update()
    {
        
        if (collider.IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>()) && Input.GetButton("Submit"))
        {
            LevelManager.Instance.switchScene(target);
        }

    }
}
