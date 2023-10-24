using System.Collections.Generic;
using UnityEngine;

public class RockDisaster : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private FloatingRocks rockPrefab;
    [SerializeField] private Transform leftCreationPoint;
    [SerializeField] private Transform rightCreationPoint;

    [Header("Configuration")]
    [SerializeField] private float minRockCount = 5;
    [SerializeField] private float maxRockCount = 15;

    [SerializeField] private float minRockSize = 2;
    [SerializeField] private float maxRockSize = 10;

    [SerializeField] private float timeBetweenWaves = 60;
    [SerializeField] private float timeBetweenWavesVariance = 5f;

    private float timeUntilNextWave;

    private readonly List<FloatingRocks> rocks = new();

    private void Update()
    {
        timeUntilNextWave -= Time.deltaTime;
        if (timeUntilNextWave < 0)
        {
            SpawnRocks();

            timeUntilNextWave = timeBetweenWaves + Random.Range(-timeBetweenWavesVariance, timeBetweenWavesVariance);
        }
    }

    private void SpawnRocks()
    {
        var rockCount = Random.Range(minRockCount, maxRockCount + 1);

        for (var i = 0; i < rockCount; i++)
        {
            var randomScale = Random.Range(minRockSize, maxRockSize);
            var spawnPoint = Vector3.Lerp(leftCreationPoint.localPosition, rightCreationPoint.localPosition, Random.value);

            var rock = Instantiate(rockPrefab, spawnPoint, Quaternion.identity);
            rocks.Add(rock);
            rock.transform.localScale = new Vector3(randomScale, randomScale, 0);
        }
    }
}
