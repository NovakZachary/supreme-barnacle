using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public InteractableSector CurrentInteractable;
    public bool interacting = false;

    public void CollidingWithInteractable(InteractableSector sector)
    {
        if (interacting)
        {
            return;
        }
        
        if (CurrentInteractable == null || sector.Priority >= CurrentInteractable.Priority)
        {
            CurrentInteractable = sector;
        }
    }
    
    public void StopCollidingWithInteractable(InteractableSector sector)
    {
        if (CurrentInteractable == sector && interacting)
        {
            CurrentInteractable = null;
        }
    }
    
    protected virtual void Update()
    {
        if (CurrentInteractable == null)
        {
            return;
        }
        
        if (Input.GetKeyDown(CurrentInteractable.GetInteractKey()))
        {
            Debug.Log($"Player has interacted with {CurrentInteractable.name}");
            
            if (!interacting)
            {
                Debug.Log($"Interacting");
                CurrentInteractable.StartInteracting();
                interacting = true;
            }
            else
            {
                Debug.Log($"Stop Interacting");
                CurrentInteractable.StopInteracting();
                interacting = false;
            }
        }
    }
}
