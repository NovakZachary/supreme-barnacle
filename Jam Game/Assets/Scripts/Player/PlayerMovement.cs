using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Serializable]
    public struct DrunkenessLayer
    {
        public float amplitude;
        public float speed;
        public float offset;
        public bool multiply;
        public bool perlin;
    }

    [Header("Dependencies")]
    public Rigidbody2D rb;

    [Header("Configuration")]
    public float movementSpeed = 8f;
    public float movementSpeedSmoothTime = 0.03f;

    // This code totally looks drunk
    [Header("Drunkeness")]
    public List<DrunkenessLayer> drunkenessLayers = new();
    public float drunkenessSpeedMultiplier = 1;

    [Space]
    [Range(0, 360)]
    public float drunkenessMaxAngle = 1;
    [Range(0, 1)]
    public float drunkenessMovementSpeedIntensity = 1;

    /// <summary>
    /// It's hard to calculate what the max value for drunkeness is.
    /// <para/>
    /// This is useful for normalizing the drunkeness values back to a [0,1] or [-1,1] range.
    /// </summary>
    private float maxDrunkenessEncountered = float.Epsilon;
    private Vector2 velocitySmoothing;

    private float totalSlipperyness = 0;

    private void Update()
    {
        var targetVelocity = Vector2.zero;
        var targetMovementSpeed = movementSpeed;
        var effectiveMovementSpeedSmoothTime = movementSpeedSmoothTime;

        // Get input
        targetVelocity.x -= Input.GetKey(ShipState.Instance.input.moveLeft) ? 1 : 0;
        targetVelocity.x += Input.GetKey(ShipState.Instance.input.moveRight) ? 1 : 0;
        targetVelocity.y += Input.GetKey(ShipState.Instance.input.moveUp) ? 1 : 0;
        targetVelocity.y -= Input.GetKey(ShipState.Instance.input.moveDown) ? 1 : 0;
        targetVelocity = Vector2.ClampMagnitude(targetVelocity, 1);

        // Apply drunkeness
        ShipState.Instance.playerDrunkenessNoise = CalculateDrunkeness();

        ApplyDrunkeness(ref targetVelocity, ref targetMovementSpeed);

        // Apply movement speed
        targetVelocity *= targetMovementSpeed;

        // Apply slipperyness
        effectiveMovementSpeedSmoothTime += totalSlipperyness;

        // Apply velocity
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocitySmoothing, effectiveMovementSpeedSmoothTime);
    }

    private float CalculateDrunkeness()
    {
        // Calculate effects of layering noises and waves
        var drunkeness = 0f;
        foreach (var layer in drunkenessLayers)
        {
            float layerValue;
            if (layer.perlin)
            {
                layerValue = (Mathf.PerlinNoise1D(Time.time * layer.speed * drunkenessSpeedMultiplier + layer.offset) - 0.5f) * 2 * layer.amplitude;
            }
            else
            {
                layerValue = Mathf.Sin(Time.time * layer.speed * drunkenessSpeedMultiplier + layer.offset) * layer.amplitude;
            }

            if (layer.multiply)
            {
                drunkeness *= layerValue;
            }
            else
            {
                drunkeness += layerValue;
            }
        }

        // Normalize value back to [-1,1] range
        maxDrunkenessEncountered = Mathf.Max(drunkeness, maxDrunkenessEncountered);
        drunkeness /= maxDrunkenessEncountered;

        return drunkeness;
    }

    private void ApplyDrunkeness(ref Vector2 targetVelocity, ref float targetMovementSpeed)
    {
        var drunkeness = ShipState.Instance.playerDrunkenessNoise;

        // Apply global drunkeness value
        drunkeness *= ShipState.Instance.playerDrunkeness;

        targetMovementSpeed = Mathf.Lerp(targetMovementSpeed, Mathf.Lerp(0, targetMovementSpeed, (drunkeness + 1) / 2), drunkenessMovementSpeedIntensity);
        targetVelocity = Quaternion.AngleAxis(drunkeness * drunkenessMaxAngle, Vector3.forward) * targetVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out SlipperyArea area))
        {
            totalSlipperyness += area.slipperyness;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out SlipperyArea area))
        {
            totalSlipperyness -= area.slipperyness;
        }
    }
}
