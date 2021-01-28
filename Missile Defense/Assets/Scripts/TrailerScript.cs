using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerScript : MonoBehaviour
{
    public GameObject EnemyMissile;
    public GameObject MissileHolder;
    public GameObject MissileHolderBase;

    // Update is called once per frame
    void Update()
    {
        if (EnemyMissile != null)
        {
            EnemyMissile = GameObject.Find("EnemyMissile(Clone)");
        }
        //Vector3 rotateMissileHolder = new Vector3(0,0.1f,0);
        //Vector3 rotateMissileHolderBase = new Vector3(0,0,0.1f);
        //MissileHolder.transform.Rotate(rotateMissileHolder);
        //MissileHolderBase.transform.Rotate(rotateMissileHolderBase);

        MissileHolder.transform.LookAt(EnemyMissile.transform);
    }
}
