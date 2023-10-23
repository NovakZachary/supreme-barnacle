using UnityEngine;

public class LowerSail : InteractableSector
{
    [SerializeField] private float smoothTimeToLower = 5f;
    
    private float verticalVelocity = 0f;
    
    protected override void Update()
    {
        base.Update();
        if (PlayerIsInteracting)
        {
            ShipState.Instance.shipSpeed = new Vector2(
                ShipState.Instance.shipSpeed.x,
                Mathf.SmoothStep(ShipState.Instance.shipSpeed.y, ShipState.Instance.minYSpeed, smoothTimeToLower)
            );
        }
    }
}
