using UnityEngine;

public class WaterBucketProjectile : MonoBehaviour
{
    [SerializeField] private GameObject puddlePrefab;
    [SerializeField] private Transform bucket;

    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private float puddleSpawnDistance = 1.5f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 45f;

    private float distanceTraveled = 0;
    private bool hasPuddleSpawned = false;

    private void Update()
    {
        var distance = speed * Time.deltaTime;
        distanceTraveled += distance;

        transform.position += transform.right * distance;
        bucket.transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);

        if (!hasPuddleSpawned && distanceTraveled > puddleSpawnDistance)
        {
            hasPuddleSpawned = true;
            Instantiate(puddlePrefab, transform.position, Quaternion.identity);
        }

        if (distanceTraveled > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
