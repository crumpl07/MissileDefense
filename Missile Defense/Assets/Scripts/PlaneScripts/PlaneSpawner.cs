using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    public GameObject Terrain;
    public GameObject PlanePrefab;
    public float height = 50;
    public float spawnTime = 10;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(planeWave());
    }

    private void spawnPlane()
    {
        GameObject plane = Instantiate(PlanePrefab) as GameObject;
        plane.transform.position = (new Vector3(Random.Range(Terrain.transform.position.x, Terrain.transform.position.x + 1000),
                                                Terrain.transform.position.y + height,
                                                Terrain.transform.position.z + 999));
    }

    IEnumerator planeWave()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTime);
            spawnPlane();
        }
    }
}
