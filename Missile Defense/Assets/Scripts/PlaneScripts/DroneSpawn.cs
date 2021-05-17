using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawn : MonoBehaviour
{
    public GameObject Terrain;
    public GameObject DronePrefab;
    public int height;
    private int spawnTime = 12;
    void Start()
    {
        StartCoroutine(planeWave());
    }

    private void spawnDrone()
    {
        int randNum = 0;
        randNum = Random.Range(1, 2);
        switch (randNum)
        {
            case 1:
                SpawnRight(DronePrefab);
                break;
            case 2:
                SpawnLeft(DronePrefab);
                break;
        }
    }

    IEnumerator missileWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            spawnDrone();
        }
    }

    void SpawnLeft(GameObject enemyMissile)
    {
        enemyMissile.transform.position = (new Vector3(
                                                       Terrain.transform.position.x,
                                                       Terrain.transform.position.y + height,
                                                       Random.Range(Terrain.transform.position.z + 1000, Terrain.transform.position.z + 3000)));
    }

    void SpawnRight(GameObject enemyMissile)
    {
        enemyMissile.transform.position = (new Vector3(
                                                        Terrain.transform.position.x + 3000,
                                                        Terrain.transform.position.y + height,
                                                        Random.Range(Terrain.transform.position.z + 1000, Terrain.transform.position.z + 3000)));
    }

    IEnumerator planeWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            spawnDrone();
        }
    }
}
