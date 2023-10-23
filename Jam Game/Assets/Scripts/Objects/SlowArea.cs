using UnityEngine;

public class SlowArea : MonoBehaviour
{
    [Tooltip("How much to slow the player by.")]
    [Range(0, 1)]
    public float movementSpeedMultiplier = 1f;
}
