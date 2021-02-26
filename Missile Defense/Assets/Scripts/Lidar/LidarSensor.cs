
using System;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : MonoBehaviour {

    private float lastUpdate = 0;

    List<Laser> lasers = new List<Laser>();
    
    public float horizontalAngle;

    [Range(0, 100)]
    public int numberOfLasers = 2;
    [Range (0.0f, 25.0f)]
    public float rotationSpeedHz = 1.0f;
    
    public float rotationAnglePerStep = 45.0f;
    [Range (5.0f, 700.0f)]
    public float rayDistance = 100f;
    [Range (1.0f, 40.0f)]
    public float upperFOV = 30f;
    [Range (1.0f, 40.0f)]
    public float lowerFOV = 30f;
    
    public float offset = 0.001f;
    [Range(-60.0f, 60.0f)]
    public float upperNormal = 30f;
    [Range(-60.0f, 60.0f)]
    public float lowerNormal = 30f;

    public static event NewPoints OnScanned;
    public delegate void NewPoints(float time, LinkedList<SphericalCoordinate> data);
    LinkedList<SphericalCoordinate> hits;

    public float lapTime = 0;

    private bool isPlaying = false;

    public GameObject pointCloudObject;
    private float previousUpdate;

    private float lastLapTime;

    public GameObject lineDrawerPrefab;

    private void Start()
    {
        lastLapTime = 0;
        hits = new LinkedList<SphericalCoordinate>();

        InitiateLasers();
    }



    private void InitiateLasers()
    {
        if (lasers != null)
        {

             foreach (Laser laser in lasers)
             {

               // Destroy(1.GetRenderLine().gameObject);
             }
        }

            lasers = new List<Laser>();

            float upperTotalAngle = upperFOV / 2;
            float lowerTotalAngle = lowerFOV / 2;
            float upperAngle = upperFOV / (numberOfLasers / 2);
            float lowerAngle = lowerFOV / (numberOfLasers / 2);
            offset = (offset / 100) / 2; // Convert offset to centimeters.
            for (int i = 0; i < numberOfLasers; i++)
            {
                GameObject lineDrawer = Instantiate(lineDrawerPrefab);
                lineDrawer.transform.parent = gameObject.transform; // Set parent of drawer to this gameObject.
                if (i < numberOfLasers / 2)
                {
                    lasers.Add(new Laser(gameObject, lowerTotalAngle + lowerNormal, rayDistance, -offset, lineDrawer, i));

                    lowerTotalAngle -= lowerAngle;
                }
                else
                {
                    lasers.Add(new Laser(gameObject, upperTotalAngle - upperNormal, rayDistance, offset, lineDrawer, i));
                    upperTotalAngle -= upperAngle;
                }
            }

            isPlaying = true;
        }




    /* public void PauseSensor(bool simulationModeOn)
       {
           if (!simulationModeOn)
           {
               isPlaying = simulationModeOn;
           }
       } */

    // Update is called once per frame 
    private void Update()
    {
        CollisionCheck();
        // For debugging, shows visible ray in real time.

//        foreach (Laser laser in lasers)
  //      {
  //      laser.DebugDrawRay();
  //    }
    }

    private void CollisionCheck()
    {
        RaycastHit hit;

        
    }
        
     


    private void FixedUpdate()
    {
        hits = new LinkedList<SphericalCoordinate>();

        if (!isPlaying)
        {
            return;
        }

        float numberOfStepsNeededInOneLap = 360 / Mathf.Abs(rotationAnglePerStep);
        float numberOfStepsPossible = 1 / Time.fixedDeltaTime / 5;
        float precalculateIterations = 1;

        // Check if we need to precalculate steps.
        if (numberOfStepsNeededInOneLap > numberOfStepsPossible)
        {
            precalculateIterations = (int)(numberOfStepsNeededInOneLap / numberOfStepsPossible);
            if (360 % precalculateIterations != 0)
            {
                precalculateIterations += 360 % precalculateIterations;
            }
        }

        // Check if it is time to step. Example: 2hz = 2 rotations in a second.
        if (Time.fixedTime - lastUpdate > (1 / (numberOfStepsNeededInOneLap) / rotationSpeedHz) * precalculateIterations)
        {
            // Update current execution time.
            lastUpdate = Time.fixedTime;

            for (int i = 0; i < precalculateIterations; i++)
            {
                // Perform rotation.
                transform.Rotate(0, rotationAnglePerStep, 0);
                horizontalAngle += rotationAnglePerStep; // Keep track of our current rotation.
                if (horizontalAngle >= 360)
                {
                    horizontalAngle -= 360;
                    //GameObject.Find("RotSpeedText").GetComponent<Text>().text =  "" + (1/(Time.fixedTime - lastLapTime));
                    lastLapTime = Time.fixedTime;

                }


                // Execute lasers.
                foreach (Laser laser in lasers)
                {
                    RaycastHit hit = laser.ShootRay();
                    float distance = hit.distance;
                    if (distance != 0) // Didn't hit anything, don't add to list.
                    {
                        if (hit.collider)
                        {
                            Debug.Log(hit.collider.transform.position);
                            Debug.Log(hit.collider.name);
                            
                        }
                        float verticalAngle = laser.GetVerticalAngle();
                        hits.AddLast(new SphericalCoordinate(distance, verticalAngle, horizontalAngle, hit.point, laser.GetLaserId()));
                    }
                }
            }


            // Notify listeners that the lidar sensor have scanned points. 
            //if (OnScanned != null  && pointCloudObject != null && pointCloudObject.activeInHierarchy)
            //{
           // OnScanned(lastLapTime, hits);
            //}

        }
    }



}



