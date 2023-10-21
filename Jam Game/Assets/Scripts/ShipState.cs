using UnityEngine;

public class ShipState : SingletonBehaviour<ShipState>
{
    // Put global values here
    // Eg: Ship health, time, whether the player is drunk/etc, etc

    [Tooltip("How drunk the player is. Affects player movement.")]
    [Range(0, 1)]
    public float playerDrunkeness = 0;

    public float horizontalSpeed = 0;
}
