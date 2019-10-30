using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float followDistance;
    [Range(0, 1)]
    public float lerp;
    public Camera c;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if ((c.transform.position - transform.position).sqrMagnitude > followDistance)
        {
            c.transform.position = Vector2.Lerp(c.transform.position, transform.position, lerp);
        }*/
    }
}
