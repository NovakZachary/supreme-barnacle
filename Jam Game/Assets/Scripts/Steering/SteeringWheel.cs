using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Update is called once per frame
    void Update()
    {
        if (playerMovement == null) return;
        
        if (Input.GetKey(InteractKey))
        {
            //TODO: Guaranteed way to disable/lock player decided by player. Perhaps state machine.
            playerMovement.enabled = false;
            playerSteering = true;
        }
        
        if (playerSteering)
        {
            //Left
            if (Input.GetKey(KeyCode.A))
            {
                float targetSpeed = -turnStrength; 
                ShipState.Instance.shipHorizontalSpeed = Mathf.SmoothDamp(ShipState.Instance.shipHorizontalSpeed, targetSpeed, ref horizontalVelocity, steeringSmoothTime);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                float targetSpeed = turnStrength; 
                ShipState.Instance.shipHorizontalSpeed = Mathf.SmoothDamp(ShipState.Instance.shipHorizontalSpeed, targetSpeed, ref horizontalVelocity, steeringSmoothTime);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == playerMovement.gameObject)
        {
            playerMovement = null;
        }
    }
}
