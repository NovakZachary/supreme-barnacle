using UnityEngine;

public class Drag : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 drag;

    private void FixedUpdate()
    {
        var velocity = rb.velocity;
        velocity.x = velocity.x * ( 1 - Time.deltaTime * drag.x);
        velocity.y = velocity.y * ( 1 - Time.deltaTime * drag.y);

        rb.velocity = velocity;
    }
}
