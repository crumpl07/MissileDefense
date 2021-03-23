using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPlane : MonoBehaviour
{

    public float speed = 20;
    private Rigidbody rb;
    public GameObject Terrain;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, -speed);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < Terrain.transform.position.z ||
           transform.position.z > Terrain.transform.position.z + 3000)
        {
            Destroy(this.gameObject);
        }
    }
}
