using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SteeringWheel : MonoBehaviour
{
    public KeyCode InteractKey;
    
    private PlayerMovement playerMovement;
    private bool playerSteering = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out var player) && player != null)
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
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerMovement.gameObject)
        {
            playerMovement = null;
        }
    }
}
