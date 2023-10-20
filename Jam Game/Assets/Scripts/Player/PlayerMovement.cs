using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] public Rigidbody2D rb;

    [Header("Configuration")]
    [SerializeField] public float movementSpeed = 8f;
    [SerializeField] public float movementSpeedSmoothTime = 0.03f;

    private Vector2 velocitySmoothing;

    private void Update()
    {
        var targetVelocity = Vector2.zero;
        targetVelocity.x -= Input.GetKey(KeyCode.A) ? 1 : 0;
        targetVelocity.x += Input.GetKey(KeyCode.D) ? 1 : 0;
        targetVelocity.y += Input.GetKey(KeyCode.W) ? 1 : 0;
        targetVelocity.y -= Input.GetKey(KeyCode.S) ? 1 : 0;

        targetVelocity = Vector2.ClampMagnitude(targetVelocity, 1);

        targetVelocity *= movementSpeed;

        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocitySmoothing, movementSpeedSmoothTime);
    }
}
