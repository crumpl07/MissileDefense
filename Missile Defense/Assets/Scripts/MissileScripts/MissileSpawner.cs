using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{

    public GameObject Terrain;
    public GameObject MissilePrefab;
    public GameObject EnemyMissilePrefab;
    public Transform SAMSite;

    public float heightOfEnemyMissile = 200;
    public float spawnTime = 10;

    //Find the trajectory of the missile
    void Start()
    {
        StartCoroutine(missileWave());
    }

    private void spawnMissile()
    {
        GameObject missile = Instantiate(MissilePrefab) as GameObject;
        GameObject enemyMissile = Instantiate(EnemyMissilePrefab) as GameObject;

        
        missile.transform.position = (new Vector3(SAMSite.position.x,
                                                  SAMSite.position.y+3,
                                                  SAMSite.position.z));

        enemyMissile.transform.position = (new Vector3(Random.Range(Terrain.transform.position.x, Terrain.transform.position.x + 1000),
                                        Terrain.transform.position.y + heightOfEnemyMissile,
                                        Terrain.transform.position.z + 999));
    }

    IEnumerator missileWave()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTime);
            spawnMissile();
        }
    }
}
