using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarFixRotation : MonoBehaviour
{

    private Vector3 scale;

    void Start()
    {
        scale = transform.localScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(transform.parent.transform.localScale.x > 0)
        {
            Vector3 flipScale = new Vector3(-scale.x,scale.y,scale.z);
            transform.localScale = flipScale;
        }
        else
            transform.localScale = scale;
    }
}
