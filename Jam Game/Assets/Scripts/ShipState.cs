using System.Collections.Generic;
using UnityEngine;

public class ShipState : SingletonBehaviour<ShipState>
{
    // Put global values here
    // Eg: Ship health, time, whether the player is drunk/etc, etc

    [Header("Input")]
    public PlayerInputMap input = new();

    [Header("Drunkeness")]
    [Tooltip("How drunk the player is. Affects player movement.")]
    [Range(0, 1)]
    public float playerDrunkeness = 0;

    [Tooltip("Automatically calculated. Higher when the player is more drunk.")]
    [Range(-1, 1)]
    public float playerDrunkenessNoise = 0;

    [Header("Player movement")]
    public HashSet<object> stopPlayerMovementRequests = new HashSet<object>();

    [Header("Ship movement")]
    public Vector2 shipSpeed = Vector2.zero;
}
