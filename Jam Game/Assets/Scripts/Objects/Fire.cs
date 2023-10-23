using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Fire : MonoBehaviour
{
    private static Collider2D[] ColliderBuffer = new Collider2D[10];

    public FireData fireData;
    public float spreadAttemptCooldown = 1f;
    public float spreadChance = 0.25f;

    [FormerlySerializedAs("checkRadius")]
    public float spreadCheckRadius = 0.25f;
    [FormerlySerializedAs("checkRange")]
    public float spreadRange = 0.5f;

    public float damage;
    public float damageRadius;
    public float damageCooldown;

    private float spreadTimer;
    private float damageTimer;

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

        damageTimer += Time.deltaTime;
        if (damageTimer > damageCooldown)
        {
            damageTimer = 0;

            DamageSurroundingObjects();
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
        var targetPosition = (Vector2)transform.position + direction * spreadRange;

        var filter = new ContactFilter2D()
        {
            useTriggers = true,
        };

        var resultCount = Physics2D.OverlapCircle(targetPosition, spreadCheckRadius, filter, ColliderBuffer);
        if (resultCount == ColliderBuffer.Length)
        {
            return;
        }

        for (var i = 0; i < resultCount; i++)
        {
            var other = ColliderBuffer[i];
            if (other.TryGetComponent(out BlocksFire _))
            {
                return;
            }
        }

        Instantiate(fireData.firePrefab, targetPosition, Quaternion.identity);
    }

    private void DamageSurroundingObjects()
    {
        var filter = new ContactFilter2D()
        {
            useTriggers = true,
        };

        var resultCount = Physics2D.OverlapCircle(transform.position, damageRadius, filter, ColliderBuffer);
        for (var i = 0; i < resultCount; i++)
        {
            var other = ColliderBuffer[i];
            if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out ShipComponent component))
            {
                component.health -= damage;
            }
        }
    }
}
