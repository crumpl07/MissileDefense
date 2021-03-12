using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyMissile : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public GameObject Terrain;
    public float speed = 150;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, -speed);
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.Rotate(-.01f, 0.0f, 0.0f, Space.Self);
        if (transform.position.z < Terrain.transform.position.z ||
            transform.position.z > Terrain.transform.position.z + 1150)
        {
            Destroy(this.gameObject);
        }
    }
}
