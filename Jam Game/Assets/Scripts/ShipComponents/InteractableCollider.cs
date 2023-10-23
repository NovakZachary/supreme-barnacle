using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class InteractableCollider : MonoBehaviour
{
    private Collider2D collider2D;
    private PlayerInteract playerInteract;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        collider2D.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody != null && col.attachedRigidbody.TryGetComponent<PlayerInteract>(out var player) && player != null)
        {
            playerInteract = player;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody != null && playerInteract != null && other.attachedRigidbody.gameObject == playerInteract.gameObject)
        {
            playerInteract = null;
        }
    }
    
    public bool IsPlayerColliding(out PlayerInteract player)
    {
        player = playerInteract;
        return playerInteract != null;
    }

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
