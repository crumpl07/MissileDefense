using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMissile : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject EnemyMissile;
    public GameObject Terrain;

    public float enemyMissileVel = 0;

    void Start()
    {
        Vector3 collisionPoint;
        if (EnemyMissile != null)
        {
            EnemyMissile = GameObject.Find("EnemyMissile(Clone)");  
        }
        collisionPoint.x = EnemyMissile.transform.position.x - 470;
        collisionPoint.y = EnemyMissile.transform.position.y - (9.8f * 4f)+3;
        collisionPoint.z = 1000 - (enemyMissileVel * 4);

        Vector3 newVelocity;
        newVelocity.x = collisionPoint.x / 3;
        newVelocity.y = collisionPoint.y / 3;
        newVelocity.z = collisionPoint.z / 3;

        rb = this.GetComponent<Rigidbody>();

        rb.velocity = new Vector3(newVelocity.x, newVelocity.y, newVelocity.z);
        //rb.transform.Rotate(0f, -40f, 0f, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.Rotate(.01f, 0.0f, 0.0f, Space.Self);
        if (transform.position.z < Terrain.transform.position.z ||
            transform.position.z > Terrain.transform.position.z + 1000)
        {
            Destroy(this.gameObject);
        }
    }
}
