using UnityEngine;

public class FireSpreader : MonoBehaviour
{
    public FireData fireData;
    public float spreadAttemptCooldown = 1f;
    public float spreadChance = 0.25f;

    public float spreadCheckRadius = 0.25f;
    public float spreadRange = 0.5f;

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
        var targetPosition = (Vector2)transform.position + direction * spreadRange;

        var filter = new ContactFilter2D()
        {
            useTriggers = true,
        };

        var resultCount = Physics2D.OverlapCircle(targetPosition, spreadCheckRadius, filter, PhysicsBuffers.Buffer);
        if (resultCount == PhysicsBuffers.Buffer.Length)
        {
            return;
        }

        for (var i = 0; i < resultCount; i++)
        {
            var other = PhysicsBuffers.Buffer[i];
            if (other.TryGetComponent(out BlocksFire _))
            {
                return;
            }
        }

        Instantiate(fireData.firePrefab, targetPosition, Quaternion.identity);
    }
}
