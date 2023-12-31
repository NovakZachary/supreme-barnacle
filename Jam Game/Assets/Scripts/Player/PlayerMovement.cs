using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dependencies")]
    public Rigidbody2D rb;

    [Header("Basic")]
    public float movementSpeed = 8f;
    public float movementSpeedSmoothTime = 0.03f;

    [Header("Sprinting")]
    public float sprintMovementSpeedMultiplier = 1.5f;

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

    [Header("Slipping")]
    public float timeBeforeSlipping = 0.5f;
    public float sprintSlipHazardMultiplier = 3f;

    [FormerlySerializedAs("slipVelocityMultiplierCurve")]
    public AnimationCurve slipVelocityCurve = new();
    public float slipVelocityMultiplier = 0.25f;
    public float slipDuration = 0.25f;
    public float slipMaxRandomAngle = 30f;

    private float timeSpentWalkingInWaterWithoutSlipping = 0;

    public HashSet<IceArea> iceAreas = new();
    public HashSet<WaterPuddleArea> waterPuddleAreas = new();
    public HashSet<SlowArea> slowAreas = new();

    [Header("Animations")]
    public Animator animator;

    private FaceDirection faceDirection = FaceDirection.Down;

    private void Update()
    {
        var movementInput = Vector2.zero;
        var targetVelocity = Vector2.zero;
        var effectiveMovementSpeed = movementSpeed;
        var effectiveMovementSpeedSmoothTime = movementSpeedSmoothTime;
        var isSprinting = Input.GetKey(ShipState.Instance.input.sprint);
        var canMove = ShipState.Instance.stopPlayerMovementRequests.Count == 0;

        // Get input
        if (canMove)
        {
            movementInput.x -= Input.GetKey(ShipState.Instance.input.moveLeft) ? 1 : 0;
            movementInput.x += Input.GetKey(ShipState.Instance.input.moveRight) ? 1 : 0;
            movementInput.y += Input.GetKey(ShipState.Instance.input.moveUp) ? 1 : 0;
            movementInput.y -= Input.GetKey(ShipState.Instance.input.moveDown) ? 1 : 0;

            targetVelocity = Vector2.ClampMagnitude(movementInput, 1);
        }

        // Apply drunkeness
        CalculateDrunkeness();
        ApplyDrunkeness(ref targetVelocity, ref effectiveMovementSpeed);

        // Apply slipperyness
        ApplyIceSlipping(ref effectiveMovementSpeedSmoothTime);
        ApplyWaterPuddleSlipping(targetVelocity, isSprinting);
        ApplySlowArea(ref effectiveMovementSpeed);
        CleanUpAreas();

        // Apply sprinting
        effectiveMovementSpeed *= isSprinting ? sprintMovementSpeedMultiplier : 1f;

        // Apply movement speed
        targetVelocity *= effectiveMovementSpeed;

        // Apply velocity
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocitySmoothing, effectiveMovementSpeedSmoothTime);

        UpdateFacingDirection(movementInput);
        UpdateAnimator(movementInput, effectiveMovementSpeed);
    }

    private void CalculateDrunkeness()
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

        // Apply global drunkeness value
        drunkeness *= ShipState.Instance.playerDrunkeness;

        ShipState.Instance.playerDrunkenessNoise = drunkeness;
    }

    private void ApplyDrunkeness(ref Vector2 targetVelocity, ref float effectiveMovementSpeed)
    {
        var drunkeness = ShipState.Instance.playerDrunkenessNoise;

        effectiveMovementSpeed = Mathf.Lerp(effectiveMovementSpeed, Mathf.Lerp(0, effectiveMovementSpeed, (drunkeness + 1) / 2), drunkenessMovementSpeedIntensity);
        targetVelocity = Quaternion.AngleAxis(drunkeness * drunkenessMaxAngle, Vector3.forward) * targetVelocity;
    }

    private void ApplyIceSlipping(ref float effectiveMovementSpeedSmoothTime)
    {
        var slipperyness = 0f;
        foreach (var area in iceAreas)
        {
            slipperyness = Mathf.Max(area.slipperyness, slipperyness);
        }

        effectiveMovementSpeedSmoothTime += slipperyness;
    }

    private void ApplyWaterPuddleSlipping(Vector2 targetVelocity, bool isSprinting)
    {
        if (targetVelocity.magnitude < 0.1f || waterPuddleAreas.Count == 0)
        {
            timeSpentWalkingInWaterWithoutSlipping = 0;

            return;
        }

        timeSpentWalkingInWaterWithoutSlipping += Time.deltaTime * (isSprinting ? sprintSlipHazardMultiplier : 1) * (ShipState.Instance.playerDrunkeness + 1);

        if (timeSpentWalkingInWaterWithoutSlipping > timeBeforeSlipping)
        {
            // Slipped
            timeSpentWalkingInWaterWithoutSlipping = 0;
            StartCoroutine(Slip());
        }
    }

    private IEnumerator Slip()
    {
        var timer = 0f;
        var initialVelocity = rb.velocity;
        var initialDirection = Quaternion.AngleAxis(Random.Range(-slipMaxRandomAngle, slipMaxRandomAngle), Vector3.forward) * initialVelocity.normalized;
        rb.velocity = Vector2.zero;

        var stopPlayerMovementRequest = "Player slipped";
        ShipState.Instance.stopPlayerMovementRequests.Add(stopPlayerMovementRequest);
        while (timer < slipDuration)
        {
            timer += Time.deltaTime;

            var slipVelocity = slipVelocityCurve.Evaluate(timer / slipDuration);
            rb.AddForce(initialVelocity.magnitude * slipVelocity * slipVelocityMultiplier * initialDirection);

            yield return null;
        }
        ShipState.Instance.stopPlayerMovementRequests.Remove(stopPlayerMovementRequest);
    }

    private void ApplySlowArea(ref float effectiveMovementSpeed)
    {
        var movementSpeedMultiplier = 1f;
        foreach (var area in slowAreas)
        {
            movementSpeedMultiplier = Mathf.Min(movementSpeedMultiplier, area.movementSpeedMultiplier);
        }

        effectiveMovementSpeed *= movementSpeedMultiplier;
    }

    private void CleanUpAreas()
    {
        iceAreas.RemoveWhere(area => !area || !area.enabled);
        waterPuddleAreas.RemoveWhere(area => !area || !area.enabled);
        slowAreas.RemoveWhere(area => !area || !area.enabled);
    }

    private void UpdateFacingDirection(Vector2 movementInput)
    {
        if (movementInput.x > 0)
        {
            faceDirection = FaceDirection.Right;

            return;
        }

        if (movementInput.x < 0)
        {
            faceDirection = FaceDirection.Left;

            return;
        }

        if (movementInput.y > 0)
        {
            faceDirection = FaceDirection.Up;

            return;
        }

        if (movementInput.y < 0)
        {
            faceDirection = FaceDirection.Down;

            return;
        }
    }

    private void UpdateAnimator(Vector2 movementInput, float effectiveMovementSpeed)
    {
        animator.SetFloat("MovementSpeed", effectiveMovementSpeed);

        var isWalking = movementInput != Vector2.zero;
        if (isWalking)
        {
            switch (faceDirection)
            {
                case FaceDirection.Down:
                {
                    animator.Play("WalkDown");

                    break;
                }
                case FaceDirection.Up:
                {
                    animator.Play("WalkUp");

                    break;
                }
                case FaceDirection.Left:
                {
                    animator.Play("WalkLeft");

                    break;
                }
                case FaceDirection.Right:
                {
                    animator.Play("WalkRight");

                    break;
                }
            }
        }
        else
        {
            switch (faceDirection)
            {
                case FaceDirection.Down:
                {
                    animator.Play("IdleDown");

                    break;
                }
                case FaceDirection.Up:
                {
                    animator.Play("IdleUp");

                    break;
                }
                case FaceDirection.Left:
                {
                    animator.Play("IdleLeft");

                    break;
                }
                case FaceDirection.Right:
                {
                    animator.Play("IdleRight");

                    break;
                }
            }
        }
    }

    [Serializable]
    public struct DrunkenessLayer
    {
        public float amplitude;
        public float speed;
        public float offset;
        public bool multiply;
        public bool perlin;
    }

    private enum FaceDirection
    {
        Down,
        Up,
        Left,
        Right,
    }
}
