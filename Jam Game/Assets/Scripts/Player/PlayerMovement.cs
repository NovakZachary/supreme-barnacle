using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] public Rigidbody2D rb;

    [Header("Configuration")]
    [SerializeField] public float movementSpeed = 8f;
    [SerializeField] public float movementSpeedSmoothTime = 0.03f;

    [SerializeField] public float drunkenessSpeed = 1f;
    [SerializeField] public float drunkenessIntensity = 30f;

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
        var drunkenessNoise = Mathf.PerlinNoise1D(Time.time * drunkenessSpeed).Dump();
        var drunkenessAngleVariance = ShipState.Instance.playerDrunkeness * drunkenessIntensity * drunkenessNoise;
        drunkenessAngleVariance.Dump();
        targetVelocity.Dump();
        targetVelocity = Quaternion.AngleAxis(drunkenessAngleVariance, Vector3.right) * targetVelocity;
        targetVelocity.Dump();

        // Apply velocity
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocitySmoothing, movementSpeedSmoothTime);
    }
}
