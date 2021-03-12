using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMissiles : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject EnemyMissile;
    public GameObject Terrain;
    public GameObject SAMsite;
    public float enemyVelocity;
    private Vector3 colPoint;
    private float colDistance;
    void Start()
    {

        if (EnemyMissile != null)
        {
            EnemyMissile = GameObject.Find("EnemyMissile(Clone)");
            GameObject Explosion = GameObject.Find("BigExplosionEffect(Clone)");
            Destroy(Explosion);
        }

        rb = this.GetComponent<Rigidbody>();

        //Find the colPoint
        colPoint.x = EnemyMissile.transform.position.x;
        colPoint.y = EnemyMissile.transform.position.y - (14.633f * 3);
        colPoint.z = 998 - (enemyVelocity * 3);

        //Find the distance between the SAM site and collision point
        float x = Mathf.Pow((colPoint.x - 500),2);
        float y = Mathf.Pow((colPoint.y - 43),2);
        float z = Mathf.Pow((colPoint.z - 100),2);
        colDistance = Mathf.Sqrt(x + y + z);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.transform.forward * (colDistance / 3);

        rb.transform.LookAt(colPoint);

        if (transform.position.z < Terrain.transform.position.z ||  
            transform.position.z > Terrain.transform.position.z + 1000)
        {
            Destroy(this.gameObject);
        }
    }
}
