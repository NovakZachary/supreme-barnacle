using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class InteractableCollider : MonoBehaviour
{
    private Collider2D collider2D;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        collider2D.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody != null && col.attachedRigidbody.TryGetComponent<PlayerMovement>(out var player) && player != null)
        {
            playerMovement = player;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody != null && playerMovement != null && other.attachedRigidbody.gameObject == playerMovement.gameObject)
        {
            playerMovement = null;
        }
    }
    
    public bool IsPlayerColliding(out PlayerMovement player)
    {
        player = playerMovement;
        return playerMovement != null;
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
