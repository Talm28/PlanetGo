using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    float angle;

    // Update is called once per frame
    void Update()
    {
        angle = Vector2.Angle(new Vector2(0,1), transform.position);

        // Fix angel for x < 0
        if(transform.position.x > 0)
            angle = -angle;

        transform.eulerAngles = new Vector3(0,0,angle);
    }
}
