using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyMissile : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public GameObject Terrain;
    public GameObject Base;
    public float speed = 150;
    private float acceleration;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        //rb.velocity = new Vector3(0, 0, -speed);
    }

    // Update is called once per frame
    void Update()
    {
        if(speed < 250)
        {
            speed += .1f;
        }
        print(speed);
        transform.LookAt(Base.transform);
        rb.velocity = transform.forward * speed ;

        if (transform.position.z < Terrain.transform.position.z ||
            transform.position.z > Terrain.transform.position.z + 3000)
        {
            Destroy(this.gameObject);
        }
    }
}
