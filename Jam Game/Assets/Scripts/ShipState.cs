using System.Collections.Generic;
using UnityEngine;

public class ShipState : SingletonBehaviour<ShipState>
{
    // Put global values here
    // Eg: Ship health, time, whether the player is drunk/etc, etc

    [Tooltip("How drunk the player is. Affects player movement.")]
    [Range(0, 1)]
    public float playerDrunkeness = 0;

    [Tooltip("Automatically calculated. Higher when the player is more drunk.")]
    [Range(-1, 1)]
    public float playerDrunkenessNoise = 0;

    public HashSet<object> stopPlayerRequests = new HashSet<object>();

    public PlayerInputMap input;
    
    public Vector2 shipSpeed = Vector2.zero;
}
