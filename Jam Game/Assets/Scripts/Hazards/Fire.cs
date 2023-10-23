using UnityEngine;
using Random = UnityEngine.Random;

public class Fire : MonoBehaviour
{
    private static Collider2D[] ColliderBuffer = new Collider2D[10];

    public FireData fireData;
    public float spreadAttemptCooldown = 1f;
    public float spreadChance = 0.25f;

    public float checkRadius = 0.25f;
    public float checkRange = 0.5f;

    private float spreadTimer;

    private void Start()
    {
        spreadTimer = Random.Range(0, spreadAttemptCooldown);
    }

    private void Update()
    {
        spreadTimer += Time.deltaTime;
        if (spreadTimer > spreadAttemptCooldown)
        {
            spreadTimer = 0;

            AttemptSpread();
        }
    }

    private void AttemptSpread()
    {
        var spreadRoll = Random.value;
        if (spreadRoll > spreadChance)
        {
            return;
        }

        var directionAngle = Random.Range(0, 2 * Mathf.PI);
        var direction = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
        var targetPosition = (Vector2)transform.position + direction * checkRange;

        var filter = new ContactFilter2D()
        {
            useTriggers = true,
        };

        var resultCount = Physics2D.OverlapCircle(targetPosition, checkRadius, filter, ColliderBuffer);
        if (resultCount == ColliderBuffer.Length)
        {
            return;
        }

        for (var i = 0; i < resultCount; i++)
        {
            var other = ColliderBuffer[i];
            if (!other.isTrigger || other.TryGetComponent(out Fire fire))
            {
                return;
            }
        }

        Instantiate(fireData.firePrefab, targetPosition, Quaternion.identity);
    }
}
