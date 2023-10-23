using UnityEngine;

public class RaiseSail : InteractableSector
{
    [SerializeField] private float smoothTimeToRaise = 5f;

    protected override void Update()
    {
        base.Update();
        if (PlayerIsInteracting)
        {
            ShipState.Instance.shipSpeed = new Vector2(
                ShipState.Instance.shipSpeed.x,
                Mathf.SmoothStep(ShipState.Instance.shipSpeed.y, ShipState.Instance.maxYSpeed, smoothTimeToRaise)
            );
        }
    }
}
