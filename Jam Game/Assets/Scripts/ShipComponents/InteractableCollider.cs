using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class InteractableCollider : MonoBehaviour
{
    private Collider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        collider2D.isTrigger = true;
    }

    // This really should be here. Single usage in Hull
    public Vector3 RandomPointWithinColliderArea()
    {
        var bounds = collider2D.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
