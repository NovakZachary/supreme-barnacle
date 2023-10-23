using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockDisaster : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private FloatingRocks rockPrefab;
    [SerializeField] private Transform leftCreationPoint;
    [SerializeField] private Transform rightCreationPoint;
    
    [Header("Configuration")]
    [SerializeField] private float minTimeDuringWaves = 20;
    [SerializeField] private float maxTimeDuringWaves = 60;
    [SerializeField] private float spawnInterval = 20;

    [SerializeField] private float minDistanceBetweenRocks = 15;
    [SerializeField] private float maxDistanceBetweenRocks = 40;
    
    [SerializeField] private float minRockSize = 2;
    [SerializeField] private float maxRockSize = 10;

    [SerializeField] private float minTimeBetweenWaves = 30;
    [SerializeField] private float maxTimeBetweenWaves = 60;

    private float previousTimeDuringWave = 0;
    private float timeDuringWave;
    private float timeUntilNextWave;

    private List<FloatingRocks> rocks = new List<FloatingRocks>();

    private void Start()
    {
        StartNextWave();
    }

    private void Update()
    {
        timeDuringWave -= Time.deltaTime;
        if (timeDuringWave <= previousTimeDuringWave - spawnInterval && timeDuringWave >= 0)
        {
            SpawnRocks();
            previousTimeDuringWave = timeDuringWave;
        }

        timeUntilNextWave -= Time.deltaTime;
        if (timeUntilNextWave <= 0)
        {
            StartNextWave();
        }
    }

    private void StartNextWave()
    {
        timeDuringWave = Random.Range(minTimeDuringWaves, maxTimeDuringWaves);
        previousTimeDuringWave = timeDuringWave;
        timeUntilNextWave = timeDuringWave + Random.Range(minTimeBetweenWaves, maxTimeBetweenWaves);
    }

    private void SpawnRocks()
    {
        var maxDistance = Vector3.Distance(leftCreationPoint.localPosition,rightCreationPoint.localPosition);
        var distance = 0f;
        while (distance < maxDistance)
        {
            var randomScale = Random.Range(minRockSize, maxRockSize);
            distance += Random.Range(minDistanceBetweenRocks, maxDistanceBetweenRocks) + randomScale;
            var spawnPoint = Vector3.Lerp(leftCreationPoint.localPosition, rightCreationPoint.localPosition, distance/maxDistance);
            var rock = Instantiate(rockPrefab, spawnPoint, Quaternion.identity);
            rocks.Add(rock);
            rock.transform.localScale = new Vector3(randomScale, randomScale, 0);
        }
    }
}
