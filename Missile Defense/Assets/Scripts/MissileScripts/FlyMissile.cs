using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMissile : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject EnemyMissile;
    public GameObject SAMsite;
    public float enemyVelocity;
    private Vector3 colPoint;
    private float colDistance;
    private float missileNumber = 0;
    void Start()
    {
        MissileSpawner missileNumber = new MissileSpawner();
        if (EnemyMissile != null) 
        {
            Debug.Log("Enemy Missile: " + missileNumber.getMissileNumber());
            EnemyMissile = GameObject.Find("EnemyMissile(Clone)");
            //EnemyMissile = GameObject.Find("EnemyMissile" + missileNumber.getMissileNumber());
            GameObject Explosion = GameObject.Find("BigExplosionEffect(Clone)");
            Destroy(Explosion);
        }

        rb = this.GetComponent<Rigidbody>();

        //Find the colPoint
        colPoint.x = EnemyMissile.transform.position.x;
        colPoint.y = EnemyMissile.transform.position.y - (14.643f * 3);
        colPoint.z = 998 - (enemyVelocity * 3);

        //Find the distance between the SAM site and collision point
        float x = Mathf.Pow((colPoint.x - SAMsite.transform.position.x), 2);
        float y = Mathf.Pow((colPoint.y - SAMsite.transform.position.y), 2);
        float z = Mathf.Pow((colPoint.z - SAMsite.transform.position.z), 2);
        colDistance = Mathf.Sqrt(x + y + z);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.transform.forward * (colDistance / 3);

        rb.transform.LookAt(EnemyMissile.transform.position);
    }
}
