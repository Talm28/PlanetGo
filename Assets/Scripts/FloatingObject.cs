using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    
    Vector3 corePos;
    float radius;
    float angle;

    // Start is called before the first frame update
    void Start()
    {
        corePos = GameObject.FindGameObjectWithTag("Planet").transform.position;
        radius = (transform.position - corePos).magnitude;
        angle = Mathf.Atan2(transform.position.y - corePos.y, transform.position.x - corePos.x);

        // Fix the angle if the object is under the core (y < core.y)
        if(transform.position.y < corePos.y)
            angle += 2*Mathf.PI;
    }

    // Update is called once per frame
    void Update()
    {
        angle -= Time.deltaTime * moveSpeed;
        Vector2 newPos = new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
        transform.position = newPos;
    }
}
