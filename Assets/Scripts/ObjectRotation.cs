using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] float tickToUpdate = 0.001f;

    float angle;
    float counter = 0;

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        // Rotation update
        if(counter > tickToUpdate)
        {
            counter = 0;
            angle = Vector2.Angle(new Vector2(0,1), transform.position);

            // Fix angel for x < 0
            if(transform.position.x > 0)
                angle = -angle;

            transform.eulerAngles = new Vector3(0,0,angle);
        }
        
    }
}
