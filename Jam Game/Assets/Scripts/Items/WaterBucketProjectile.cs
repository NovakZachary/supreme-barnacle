using UnityEngine;

public class WaterBucketProjectile : MonoBehaviour
{
    [SerializeField] private Transform bucket;

    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 45f;

    private float distanceTraveled = 0;

    private void Update()
    {
        var distance = speed * Time.deltaTime;
        distanceTraveled += distance;

        transform.position += transform.right * distance;
        bucket.transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);

        if (distanceTraveled > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
