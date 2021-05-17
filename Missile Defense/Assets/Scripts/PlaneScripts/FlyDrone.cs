using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyDrone : MonoBehaviour
{
    private Rigidbody rb;
    private int speed = 30;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.transform.forward * speed;
        
    }
}
