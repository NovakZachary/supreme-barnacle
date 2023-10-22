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

    [Header("Time")]
    [Tooltip("Time survived.")]
    public float timeSurvivedSeconds = 0;
    [Tooltip("Determines how long an hour is.")]
    public float secondsPerHour = 10; // 4 min or 10 seconds per hour
    [Tooltip("The hour the game starts at. Useful for making the game start at night instead of day, etc.")]
    public int startingHour = 8;

    /// <summary>
    /// The current time in hours. Range of [0, 24).
    /// </summary>
    public float currentDayTime => (timeSurvivedSeconds / secondsPerHour + startingHour) % 24;

    /// <summary>
    /// The current hour. Range of [0, 24).
    /// </summary>
    public int currentDayHour => (int)currentDayTime;

    /// <summary>
    /// The current minute. Range of [0, 60).
    /// </summary>
    public int currentDayMinutes => (int)(currentDayTime % 1 * 60);

    /// <summary>
    /// The current time of day, normalized to be in the range of [0, 1).
    /// </summary>
    public float currentDayTimeNormalized => currentDayTime / 24;

    [Header("Ship components")]
    [Tooltip("All damage taken by the ship will be multiplied by this")]
    public float shipDamageTakenMultiplier = 1;

    [Space]
    [Tooltip("The current health of the ship. Automatically calculated from ship components.")]
    public float shipHealth = 1;
    [Tooltip("The max health of the ship. Automatically calculated from ship components.")]
    public float shipMaxHealth = 1;
    [Range(0, 1)]
    [Tooltip("The percentage of remaining health that the ship will sink at, causing the player to lose the game.")]
    public float shipSinkThreshold = 0.4f;

    /// <summary>
    /// The effective health of the ship, based on <see cref="shipHealth"/> and <see cref="shipSinkThreshold"/>. Range of [0, 1]
    /// Once this reaches 0, the ship will sink, causing the player to lose the game.
    /// </summary>
    [Range(0, 1)]
    public float shipIntegrity;

    public HashSet<ShipComponent> shipComponents = new();

    private void Update()
    {
        // Probably better to not update values from here, but it's a game jam

        timeSurvivedSeconds += Time.deltaTime;
        shipHealth = float.Epsilon;
        shipMaxHealth = float.Epsilon;
        foreach (var shipComponent in shipComponents)
        {
            if (shipComponent.isStructural)
            {
                shipHealth += Mathf.Max(0, shipComponent.health);
                shipMaxHealth += shipComponent.maxHealth;
            }
        }

        shipIntegrity = (shipHealth - shipMaxHealth * shipSinkThreshold) / (shipMaxHealth - shipMaxHealth * shipSinkThreshold);

        if (shipIntegrity < float.Epsilon)
        {
            Debug.Log("Game lost");
        }
    }
}
