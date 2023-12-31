using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    [Range(0, 1)]
    public float playerSoberingRate = 0.05f;

    [Tooltip("Automatically calculated. Higher when the player is more drunk.")]
    [Range(-1, 1)]
    public float playerDrunkenessNoise = 0;

    [Header("Player movement")]
    public HashSet<object> stopPlayerMovementRequests = new HashSet<object>();

    [FormerlySerializedAs("shipSpeed")]
    [Header("Ship movement")]
    public Vector2 shipVelocity = Vector2.zero;
    public bool MastLowered => Math.Abs(shipVelocity.y - maxYSpeed) < 0.1f;
    public bool MastRaised => Math.Abs(shipVelocity.y - minYSpeed) < 0.1f;
    public float minYSpeed = 10;
    public float maxYSpeed = 40;
    public float distanceTraveled = 0;

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
    
    public bool isNight => currentDayTime < 8 || currentDayTime > 20;

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

    private HashSet<ShipComponent> shipComponents = new();

    public IReadOnlyCollection<ShipComponent> ShipComponents => shipComponents;
    public event Action<ShipComponent> ShipComponentAdded;
    public event Action<ShipComponent> ShipComponentRemoved;

    [Header("Ship tilt")]
    [Tooltip("Multiplier to how strong gravity is.")]
    public float gravityStrength = 1f;
    [Tooltip("Minimum angle at which gravity begins affecting objects on the ship.")]
    [Range(0, 90)]
    public float gravityMinimumAngle = 5f;
    [Tooltip("Ship tilt angle in degrees.")]
    [Range(-90, 90)]
    public float shipAngle = 0;

    /// <summary>
    /// Collection of modifiers to shipAngle. Each layer/modifier is applied additively. Result is the sum of all layers.
    /// </summary>
    public HashSet<Func<float>> shipAngleLayers = new();
    
    private void Update()
    {
        UpdateShipHealth();
        UpdateShipTilt();

        if (shipIntegrity < float.Epsilon)
        {
            Debug.Log("Game lost");
        }
        
        ReduceDrunknessOverTime();

        shipVelocity.y = Mathf.Clamp(shipVelocity.y, minYSpeed, maxYSpeed);
    }

    public void RegisterShipComponent(ShipComponent component)
    {
        if (shipComponents.Add(component))
        {
            ShipComponentAdded?.Invoke(component);
        }
    }

    public void UnregisterShipComponent(ShipComponent component)
    {
        if (shipComponents.Remove(component))
        {
            ShipComponentRemoved?.Invoke(component);
        }
    }

    private void UpdateShipHealth()
    {
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
    }

    private void UpdateShipTilt()
    {
        shipAngle = 0;
        foreach (var layer in shipAngleLayers)
        {
            shipAngle += layer();
        }

        var direction = Mathf.Sign(shipAngle);
        var effectiveAngle = Mathf.Clamp(Mathf.Abs(shipAngle) - gravityMinimumAngle, 0, 90);
        Physics2D.gravity = new Vector2(direction * Mathf.Sin(effectiveAngle * Mathf.Deg2Rad) * gravityStrength * 9.81f, 0);
    }

    private void ReduceDrunknessOverTime()
    {
        Player.Instance.spriteRenderer.color = Color.Lerp(Color.white, Color.magenta, playerDrunkeness);
        playerDrunkeness = Mathf.Clamp(playerDrunkeness - (playerSoberingRate * Time.deltaTime), 0, playerDrunkeness);
    }
}
