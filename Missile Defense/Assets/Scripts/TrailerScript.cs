using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerScript : MonoBehaviour
{
    public GameObject missileHolder;
    public GameObject missileHolderBase;
    public GameObject EnemyMissile;

    private void Update()
    {
        if (EnemyMissile != null)
        {
            EnemyMissile = GameObject.Find("EnemyMissile(Clone)");
        }
        missileHolder.transform.LookAt(EnemyMissile.transform);
        missileHolderBase.transform.LookAt(EnemyMissile.transform);
    }
}
