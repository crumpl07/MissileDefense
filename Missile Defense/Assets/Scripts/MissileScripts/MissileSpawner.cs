using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{

    public GameObject Terrain;
    public GameObject MissilePrefab;
    public GameObject EnemyMissilePrefab;

    public float height = 200;
    public float xMissileLauncher = 0;
    public float yMissileLauncher = 0;
    public float zMissileLauncher = 0;
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

        
        missile.transform.position = (new Vector3(xMissileLauncher, 
                                                  yMissileLauncher + 3,
                                                  zMissileLauncher));

        enemyMissile.transform.position = (new Vector3(Random.Range(Terrain.transform.position.x, Terrain.transform.position.x + 1000),
                                        Terrain.transform.position.y + height,
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
