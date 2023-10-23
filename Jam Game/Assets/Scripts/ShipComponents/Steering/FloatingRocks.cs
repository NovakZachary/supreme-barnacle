using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class FloatingRocks : MonoBehaviour
{
    [SerializeField] private List<Sprite> randomSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [Header("Configuration")]
    public float damageOnCollision;

    [Header("Rotate around pivot")]
    [SerializeField] private float rotationDegreesPerSecond = 1f;
    [SerializeField] private Vector3 pivotPosition;

    [Header("Destroy self")]
    [SerializeField] private bool destroyWhenFarFromPivot = true;
    [SerializeField] private float distanceFromPivotUntilDestroyed = 10f;

    private void Awake()
    {
        if (randomSprite.Count > 0)
        {
            spriteRenderer.sprite = randomSprite[Random.Range(0, randomSprite.Count)];
        }
    }

    private void Update()
    {
        transform.RotateAround(pivotPosition, Vector3.forward, rotationDegreesPerSecond * Time.deltaTime);
        
        transform.position -= (Vector3)ShipState.Instance.shipVelocity * Time.deltaTime;

        if (destroyWhenFarFromPivot && Vector3.Distance(transform.position, pivotPosition) > distanceFromPivotUntilDestroyed)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent<Railing>(out var railing))
        {
            railing.DamageShip(damageOnCollision, col.GetContact(0).point);
        }
    }
}
