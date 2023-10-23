using UnityEngine;

public class WavesShipTilt : MonoBehaviour
{
    [Tooltip("How often the cycle repeats.")]
    public float period = 5;

    [Tooltip("Max tilt contributed by waves.")]
    public float amplitude = 8;

    private void OnEnable()
    {
        ShipState.Instance.shipAngleLayers.Add(CalculateShipAngle);
    }

    private void OnDisable()
    {
        ShipState.Instance.shipAngleLayers.Remove(CalculateShipAngle);
    }

    private float CalculateShipAngle()
    {
        return Mathf.Sin(Time.time * (1 / period)) * amplitude;
    }
}
