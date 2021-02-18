using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMissiles : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject EnemyMissile;
    public GameObject Terrain;
    public float enemyVelocity;
    private Vector3 colPoint;
    private Vector3 colVelocity;
    private Vector3 colAngle;
    private float distanceToCol;

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
        colPoint.y = EnemyMissile.transform.position.y - (9.8f * 3);
        colPoint.z = 1000 - enemyVelocity * 3;

        //Set the velocityVector
        colVelocity.x = colPoint.x / 3;
        colVelocity.y = colPoint.y / 3;
        colVelocity.z = colPoint.z / 3;

        //Find the colAngle
        colAngle.y = Mathf.Tan(colPoint.x / 450);
        distanceToCol = Mathf.Sqrt(Mathf.Pow(450, 2) + Mathf.Pow(Mathf.Abs(colPoint.x - 500), 2));
        colAngle.x = Mathf.Tan(colPoint.y / distanceToCol);

        rb.velocity = colVelocity;
        rb.transform.Rotate(colAngle);

    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.z < Terrain.transform.position.z ||
            transform.position.z > Terrain.transform.position.z + 1000)
        {
            Destroy(this.gameObject);
        }
    }
}
