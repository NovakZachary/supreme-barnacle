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

    private void Update()
    {
        base.Update();
        
        if (PlayerIsInteracting)
        {
            lookoutTower.EnableLookoutCamera();
            
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
        ShipState.Instance.shipSpeed = new Vector2(
                                Mathf.SmoothDamp(ShipState.Instance.shipSpeed.x, targetSpeed, ref horizontalVelocity, steeringSmoothTime),
                                ShipState.Instance.shipSpeed.y
                        );
    }
    
    private void StopTurningShip()
    {
        lookoutTower.DisableLookoutCameras();
        
        if (Mathf.Abs(ShipState.Instance.shipSpeed.x) > 0.001)
        {
            TurnShip(0);
        }
        else
        {
            ShipState.Instance.shipSpeed = new Vector2(
                0,
                ShipState.Instance.shipSpeed.y
            );
        }
    }
    
    private float CalculateShipAngle()
    {
        return ShipState.Instance.shipSpeed.x * turnShipAngleMultiplier;
    }
}
