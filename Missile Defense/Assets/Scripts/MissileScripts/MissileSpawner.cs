using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{

    public GameObject Terrain;
    public GameObject MissilePrefab;
    public GameObject EnemyMissilePrefab;
    public Transform SAMSite;

    public float heightOfEnemyMissile = 300;
    public float spawnTime = 10;

    //Find the trajectory of the missile
    void Start()
    {
        StartCoroutine(missileWave());
    }

    private void spawnEnemyMissile()
    {
        
        GameObject enemyMissile = Instantiate(EnemyMissilePrefab) as GameObject;
        GameObject missile = Instantiate(MissilePrefab) as GameObject;

        int randNum = 0;
        randNum = Random.Range(1, 3);
        switch(randNum)
        {
            case 1: SpawnBack(enemyMissile);
                break;
            case 2: SpawnLeft(enemyMissile);
                break;
            case 3: SpawnRight(enemyMissile);
                break;
        }

        
        missile.transform.position = (new Vector3(SAMSite.position.x,
                                                  SAMSite.position.y,
                                                  SAMSite.position.z));

    }

    IEnumerator missileWave()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTime);
            spawnEnemyMissile();
        }
    }

    void SpawnBack(GameObject enemyMissile)
    {
        enemyMissile.transform.position = (new Vector3(
                                        Random.Range(Terrain.transform.position.x, Terrain.transform.position.x + 3000),
                                        Terrain.transform.position.y + heightOfEnemyMissile,
                                        Terrain.transform.position.z + 3000));
    }

    void SpawnLeft(GameObject enemyMissile)
    {
        enemyMissile.transform.position = (new Vector3(
                                                       Terrain.transform.position.x, 
                                                       Terrain.transform.position.y + heightOfEnemyMissile,
                                                       Random.Range(Terrain.transform.position.z + 1000, Terrain.transform.position.z + 3000)));
    }

    void SpawnRight(GameObject enemyMissile)
    {
        enemyMissile.transform.position = (new Vector3(
                                                        Terrain.transform.position.x + 3000,
                                                        Terrain.transform.position.y + heightOfEnemyMissile,
                                                        Random.Range(Terrain.transform.position.z + 1000, Terrain.transform.position.z + 3000)));
    }
}
