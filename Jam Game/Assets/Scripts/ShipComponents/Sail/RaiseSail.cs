using UnityEngine;

public class RaiseSail : InteractableSector
{
    [SerializeField] private float raisePerSecond = 2;

    protected override void Update()
    {
        base.Update();
        if (PlayerIsInteracting)
        {
            ShipState.Instance.shipSpeed = new Vector2(
                ShipState.Instance.shipSpeed.x,
                Mathf.Clamp(ShipState.Instance.shipSpeed.y + raisePerSecond * Time.deltaTime, ShipState.Instance.minYSpeed, ShipState.Instance.maxYSpeed)
            );
        }
    }
}
