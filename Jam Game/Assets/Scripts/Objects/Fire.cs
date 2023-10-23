using UnityEngine;

public class Fire : MonoBehaviour
{
    public float damage;
    public float damageRadius;
    public float damageCooldown;

    private float damageTimer;

    private void Update()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer > damageCooldown)
        {
            damageTimer = 0;

            DamageSurroundingObjects();
        }
    }

    private void DamageSurroundingObjects()
    {
        var filter = new ContactFilter2D()
        {
            useTriggers = true,
        };

        var resultCount = Physics2D.OverlapCircle(transform.position, damageRadius, filter, PhysicsBuffers.Buffer);
        for (var i = 0; i < resultCount; i++)
        {
            var other = PhysicsBuffers.Buffer[i];
            if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out ShipComponent component))
            {
                component.health -= damage;
            }
        }
    }
}
