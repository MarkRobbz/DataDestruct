using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiSpawner : MonoBehaviour
{
    public GameObject taxiPrefab; //Located in ArrayAlleyway Scripts folder (TaxisDrivePrefab)
    public float minSpawnInterval = 2f; 
    public float maxSpawnInterval = 5f; 
    public Transform[] spawnPoints; //Transcforms of locations for spawn points

    private float timer;
    private float currentSpawnInterval;



void Start()
    {
        SetRandomSpawnInterval();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnTaxi();
            SetRandomSpawnInterval();
        }
    }

    void SpawnTaxi()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        RaycastHit hit;
        if (Physics.Raycast(spawnPoint.position, -Vector3.up, out hit))
        {
            // Calculate y position taxi's prefab height
            float prefabHeight = taxiPrefab.GetComponent<Collider>().bounds.size.y;
            Vector3 spawnPosition = hit.point + Vector3.up * (prefabHeight * 0.5f);

            // Instantiate taxis at pos
            Instantiate(taxiPrefab, spawnPosition, spawnPoint.rotation);
        }
        else
        {
            // Fallback if raycast doesn't hit anything
            Instantiate(taxiPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }


    void SetRandomSpawnInterval()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        timer = currentSpawnInterval; // Resets timer with new random interval
    }

}

