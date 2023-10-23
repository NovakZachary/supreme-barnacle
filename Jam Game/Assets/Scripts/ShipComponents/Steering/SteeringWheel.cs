using UnityEngine;

public class SteeringWheel : InteractableSector
{
    [Header("Dependencies")] 
    [SerializeField] private LookoutTower lookoutTower;
    
    [Header("Configuration")]
    [SerializeField] private float turnStrength = 1f;
    [SerializeField] private float steeringSmoothTime = 0.1f;
    [SerializeField] private float turnShipAngleMultiplier = 5f;
    
    private PlayerMovement playerMovement;
    private float horizontalVelocity;

    private void OnEnable()
    {
        ShipState.Instance.shipAngleLayers.Add(CalculateShipAngle);
    }

    private void OnDisable()
    {
        ShipState.Instance.shipAngleLayers.Remove(CalculateShipAngle);
    }

    protected override void Update()
    {
        base.Update();
        
        if (PlayerIsInteracting)
        {
            lookoutTower.LookoutRequesters.Add(this);
            
            if (Input.GetKey(ShipState.Instance.input.moveLeft))
            {
                var targetSpeed = -turnStrength;
                TurnShip(targetSpeed);
            } else if (Input.GetKey(ShipState.Instance.input.moveRight))
            {
                var targetSpeed = turnStrength; 
                TurnShip(targetSpeed);
            }
        } 
        else if(HasReset)
        {
            StopTurningShip();
        }
    }

    private void TurnShip(float targetSpeed)
    {
        ShipState.Instance.shipVelocity = new Vector2(
                                Mathf.SmoothDamp(ShipState.Instance.shipVelocity.x, targetSpeed, ref horizontalVelocity, steeringSmoothTime),
                                ShipState.Instance.shipVelocity.y
                        );
    }
    
    private void StopTurningShip()
    {
        lookoutTower.LookoutRequesters.Remove(this);
        
        if (Mathf.Abs(ShipState.Instance.shipVelocity.x) > 0.001)
        {
            TurnShip(0);
        }
        else
        {
            ShipState.Instance.shipVelocity = new Vector2(
                0,
                ShipState.Instance.shipVelocity.y
            );
        }
    }
    
    private float CalculateShipAngle()
    {
        return ShipState.Instance.shipVelocity.x * turnShipAngleMultiplier;
    }
}
