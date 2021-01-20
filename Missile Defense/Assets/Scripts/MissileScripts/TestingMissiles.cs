using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMissiles : MonoBehaviour
{
    public GameObject Terrain;
    public GameObject MissilePrefab;
    public GameObject EnemyMissilePrefab;

    public float xMissileLauncher = 0;
    public float yMissileLauncher = 0;
    public float zMissileLauncher = 0;
    public float spawnTime = 5;

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
                                                  yMissileLauncher,
                                                  zMissileLauncher));

        enemyMissile.transform.position = (new Vector3(800f, 200f, 1000f));

        GameObject explosion;
        explosion = GameObject.Find("BigExplosionEffect(Clone)");
        Destroy(explosion);
    }

    IEnumerator missileWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            spawnMissile();
        }
    }
}
