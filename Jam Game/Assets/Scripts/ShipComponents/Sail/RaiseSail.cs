using UnityEngine;

public class RaiseSail : InteractableSector
{
    [SerializeField] private float raisePerSecond = 2;

    protected override void Update()
    {
        base.Update();
        if (PlayerIsInteracting)
        {
            ShipState.Instance.shipVelocity = new Vector2(
                ShipState.Instance.shipVelocity.x,
                Mathf.Clamp(ShipState.Instance.shipVelocity.y + raisePerSecond * Time.deltaTime, ShipState.Instance.minYSpeed, ShipState.Instance.maxYSpeed)
            );
        }
    }
}
