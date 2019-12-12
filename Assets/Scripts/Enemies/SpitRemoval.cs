using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitRemoval : MonoBehaviour
{

    public float lifetime = 15f;

    private float timeAlive = 0f;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeAlive += Time.fixedDeltaTime;

        if (timeAlive > lifetime)
        {
            anim.Play("enemySlimeDisappear");
        }
        
    }

    public void getOutaHere()
    {
        if (timeAlive > lifetime)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

}
