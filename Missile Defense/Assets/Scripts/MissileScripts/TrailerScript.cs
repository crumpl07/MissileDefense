using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerScript : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject EnemyMissile;
    public GameObject MissileHolder;
    public GameObject MissileHolderBase;

    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("EnemyMissile(Clone)") != null)
        {
            EnemyMissile = GameObject.Find("EnemyMissile(Clone)");
            MissileHolder.transform.LookAt(EnemyMissile.transform);
            MissileHolderBase.transform.LookAt(EnemyMissile.transform);
        }
    }
}
