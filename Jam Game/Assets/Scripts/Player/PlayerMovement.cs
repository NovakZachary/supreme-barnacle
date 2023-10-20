using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Serializable]
    public struct DrunkenessLayer
    {
        public float Speed;
        public float Amplitude;
    }

    [Header("Dependencies")]
    public Rigidbody2D rb;

    [Header("Configuration")]
    public float movementSpeed = 8f;
    public float movementSpeedSmoothTime = 0.03f;

    public List<DrunkenessLayer> drunkenessLayers = new();
    public float drunkenessAmplitudeMultiplier = new();

    private Vector2 velocitySmoothing;

    private void Update()
    {
        // Get input
        var targetVelocity = Vector2.zero;
        targetVelocity.x -= Input.GetKey(KeyCode.A) ? 1 : 0;
        targetVelocity.x += Input.GetKey(KeyCode.D) ? 1 : 0;
        targetVelocity.y += Input.GetKey(KeyCode.W) ? 1 : 0;
        targetVelocity.y -= Input.GetKey(KeyCode.S) ? 1 : 0;

        // Clamp
        targetVelocity = Vector2.ClampMagnitude(targetVelocity, 1);

        // Apply movement speed
        targetVelocity *= movementSpeed;

        // Apply drunkeness
        var drunkeness = 0f;
        foreach (var drunkenessLayer in drunkenessLayers)
        {
            drunkeness += Mathf.Sin(Time.time * drunkenessLayer.Speed) * drunkenessLayer.Amplitude;
        }

        drunkeness *= ShipState.Instance.playerDrunkeness;
        drunkeness *= drunkenessAmplitudeMultiplier;
        targetVelocity = Quaternion.AngleAxis(drunkeness, Vector3.forward) * targetVelocity;

        // Apply velocity
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocitySmoothing, movementSpeedSmoothTime);
    }
}
