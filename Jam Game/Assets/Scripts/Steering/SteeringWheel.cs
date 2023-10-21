using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
    public KeyCode InteractKey;

    [Header("Configuration")]
    [SerializeField] private float turnStrength = 1f;
    [SerializeField] private float steeringSmoothTime = 0.1f;
    
    private PlayerMovement playerMovement;
    private bool playerSteering = false;
    private float horizontalVelocity;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hello");
        if (col.attachedRigidbody != null && col.attachedRigidbody.TryGetComponent<PlayerMovement>(out var player) && player != null)
        {
            playerMovement = player;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == playerMovement.gameObject)
        {
            playerMovement = null;
        }
    }

    private void Update()
    {
        if (playerMovement == null) return;
        
        if (Input.GetKey(ShipState.Instance.input.interact))
        {
            HandleInteract();
        }
        
        if (playerSteering)
        {
            //Left
            if (Input.GetKey(ShipState.Instance.input.moveLeft))
            {
                var targetSpeed = -turnStrength;
                TurnShip(targetSpeed);
            } 
            
            if (Input.GetKey(ShipState.Instance.input.moveRight))
            {
                var targetSpeed = turnStrength; 
                TurnShip(targetSpeed);
            }
        }
        else if(Mathf.Abs(ShipState.Instance.shipSpeed.x) > 0.001)
        {
            TurnShip(0);
        }
        else
        {
            StopTurningShip();
        }
    }

    private void HandleInteract()
    {
        if (!playerSteering)
        {
            //TODO: Guaranteed way to disable/lock player decided by player. Perhaps state machine.
            ShipState.Instance.stopPlayerRequests.Add(this);
            playerSteering = true;
        }
        else
        {
            ShipState.Instance.stopPlayerRequests.Remove(this);
            playerSteering = false;
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
        ShipState.Instance.shipSpeed = new Vector2(
            0,
            ShipState.Instance.shipSpeed.y
        ); 
    }
}
