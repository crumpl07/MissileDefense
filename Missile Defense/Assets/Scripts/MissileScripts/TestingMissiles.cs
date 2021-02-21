using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMissiles : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject EnemyMissile;
    public GameObject Terrain;
    private GameObject Explosion;
    public float enemyVelocity;
    private Vector3 colPoint;
    private float colDistance;

    void Start()
    {

        if (EnemyMissile != null)
        {
            EnemyMissile = GameObject.Find("EnemyMissile(Clone)");
            Explosion = GameObject.Find("BigExplosionEffect(Clone)");
            if(Explosion != null)
            {
                Debug.Log("Explosion: " + Explosion.transform.position);
            }
            Destroy(Explosion);
        }

        rb = this.GetComponent<Rigidbody>();

        //Find the colPoint
        colPoint.x = EnemyMissile.transform.position.x;
        colPoint.y = EnemyMissile.transform.position.y - (9.8f * 3);
        colPoint.z = 1000 - (enemyVelocity * 3);
        colDistance = Mathf.Sqrt(Mathf.Pow(colPoint.x - 500, 2) + Mathf.Pow(colPoint.y - 43, 2) + Mathf.Pow(colPoint.z - 100, 2));
        //Debug.Log(colDistance);
        Debug.Log("Collsion Point: " + colPoint);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.transform.forward * (colDistance / 3);
        rb.transform.LookAt(EnemyMissile.transform.position);
        if (transform.position.z < Terrain.transform.position.z ||
            transform.position.z > Terrain.transform.position.z + 1000)
        {
            Destroy(this.gameObject);
        }
    }
}
