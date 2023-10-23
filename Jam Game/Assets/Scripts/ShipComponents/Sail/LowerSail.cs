using UnityEngine;

public class LowerSail : InteractableSector
{
    [SerializeField] private float lowerPerSecond = 2f;
    
    private float verticalVelocity = 0f;
    
    private void Update()
    {
        if (Player.Instance.interaction.activeInteractable == this)
        {
            ShipState.Instance.shipVelocity = new Vector2(
                ShipState.Instance.shipVelocity.x,
                Mathf.Clamp(ShipState.Instance.shipVelocity.y - lowerPerSecond * Time.deltaTime, ShipState.Instance.minYSpeed, ShipState.Instance.maxYSpeed)
            );
        }
    }
}
