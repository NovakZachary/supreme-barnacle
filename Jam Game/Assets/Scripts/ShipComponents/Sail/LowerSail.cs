using UnityEngine;

public class LowerSail : InteractableSector
{
    [SerializeField] private float lowerPerSecond = 2f;
    
    private float verticalVelocity = 0f;
    
    protected override void Update()
    {
        base.Update();
        if (PlayerIsInteracting)
        {
            ShipState.Instance.shipSpeed = new Vector2(
                ShipState.Instance.shipSpeed.x,
                Mathf.Clamp(ShipState.Instance.shipSpeed.y - lowerPerSecond * Time.deltaTime, ShipState.Instance.minYSpeed, ShipState.Instance.maxYSpeed)
            );
        }
    }
}
