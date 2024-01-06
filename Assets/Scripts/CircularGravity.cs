using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularGravity : MonoBehaviour
{
    GameObject Core;
    [SerializeField] double g = 200f;

    Rigidbody2D ObjBody, CoreBody;
    Transform CoreTransform;
    float m1, m2, radius;
    Vector2 Direction;

    // Start is called before the first frame update
    void Start()
    {  
        Core = GameObject.FindGameObjectWithTag("Planet");
        CoreTransform = Core.transform;

        // Rigidbodyies
        CoreBody = Core.GetComponent<Rigidbody2D>();
        ObjBody = GetComponent<Rigidbody2D>();

        // Mass 
        m1 = ObjBody.mass;
        m2 = CoreBody.mass;

        // Radius and direction vector
        Direction = transform.position - Core.transform.position;
        radius = Vector2.Distance(transform.position,Core.transform.position);
        Direction = Direction/Direction.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Direction = transform.position - Core.transform.position;
        Direction = Direction/Direction.magnitude;
        radius = Vector2.Distance(transform.position,Core.transform.position);
        Vector2 CircularForch = (float)(g * (m1+m2)/Mathf.Pow(radius,2))* Direction;
        ObjBody.AddForce(-CircularForch);
    }
}
