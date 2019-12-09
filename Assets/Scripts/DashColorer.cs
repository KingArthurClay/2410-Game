using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashColorer : MonoBehaviour
{

    public Color dashedColor;

    private PlayerController player;
    private SpriteRenderer sprite;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();

        dashedColor.a = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.dash)
        {
            sprite.color = dashedColor;
        }
        else
        {
            sprite.color = new Color(255, 255, 255);
        }
    }
}
