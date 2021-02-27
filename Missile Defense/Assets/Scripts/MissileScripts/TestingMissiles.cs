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
            if(Explosion != null)
            {
                Debug.Log("Explosion: " + Explosion.transform.position);
            }
            Destroy(Explosion);
        }

        rb = this.GetComponent<Rigidbody>();

        //Find the colPoint
        colPoint.x = EnemyMissile.transform.position.x;
        colPoint.y = EnemyMissile.transform.position.y - (14.633f * 3);
        colPoint.z = 998 - (enemyVelocity * 3);

        float x = Mathf.Pow((colPoint.x - SAMsite.transform.position.x), 2);
        float y = Mathf.Pow((colPoint.y - SAMsite.transform.position.y), 2);
        float z = Mathf.Pow((colPoint.z - SAMsite.transform.position.z), 2);

        float sum = x + y + z;
        colDistance = Mathf.Sqrt(sum);
        Debug.Log("Collison Point: " + colPoint);
        float speed = colDistance / 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > 7.999 && Time.time < 8.003)
        {
            Debug.Log("Missile Location: " + EnemyMissile.transform.position);
        }
       rb.velocity = rb.transform.forward * (colDistance / 3);

        rb.transform.LookAt(colPoint);
        if (transform.position.z < Terrain.transform.position.z ||  
            transform.position.z > Terrain.transform.position.z + 1000)
        {
            Destroy(this.gameObject);
        }
    }
}
