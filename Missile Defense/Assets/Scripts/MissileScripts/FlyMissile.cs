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
        if (EnemyMissile != null)
        {
            EnemyMissile = GameObject.Find("EnemyMissile(Clone)");
            GameObject Explosion = GameObject.Find("BigExplosionEffect(Clone)");
            Destroy(Explosion);
        }
        
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.LookAt(EnemyMissile.transform);
        rb.velocity = rb.transform.forward * 300;

        if (transform.position.z < Terrain.transform.position.z ||
            transform.position.z > Terrain.transform.position.z + 1000)
        {
            Destroy(this.gameObject);
        }
    }
}
