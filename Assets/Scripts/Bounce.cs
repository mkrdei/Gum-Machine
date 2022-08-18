using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounciness = 1.25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        //if(transform.position.y + transform.up.y)
        //   Vector3 direction = Vector3.zero;
        Rigidbody rb = col.transform.GetComponent<Rigidbody>();
        rb.velocity = rb.velocity*bounciness;
        
    }
}
