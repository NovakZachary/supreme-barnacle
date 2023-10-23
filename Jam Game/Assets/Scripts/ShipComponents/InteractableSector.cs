using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShipComponent))]
public abstract class InteractableSector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InteractableCollider collider;
    
    [Header("Configuration")]
    [SerializeField] private float timeAfterInteractionUntilStopsWorking;

    public bool PlayerIsInteracting { get; private set; }
    public bool HasReset => resetTimer == 0;

    public ShipComponent ShipComponent { get; private set; }
    private float resetTimer;

    private void Awake()
    {
        ShipComponent = GetComponent<ShipComponent>();
        if (collider == null)
        {
            collider = GetComponentInChildren<InteractableCollider>();
            Debug.Log($"Please set Interactable Collider dependency on {gameObject.name}");
        }
    }

    protected virtual void Update()
    {
        if (collider.IsPlayerColliding(out var playerMovement))
        {
            Debug.Log($"Player is trigger colliding with interacted with {ShipComponent.displayName}");
            if (Input.GetKeyDown(GetInteractKey()))
            {
                Debug.Log($"Player has interacted with {ShipComponent.displayName}");
                if (!PlayerIsInteracting)
                {
                    StartInteracting();
                }
                else
                {
                    StopInteracting();
                }
            }
        }
        else //Player has been pushed out of interaction collider
        {
            StopInteracting();
        }

        if (!PlayerIsInteracting)
        {
            resetTimer -= Time.deltaTime;
            resetTimer = Mathf.Clamp(resetTimer, 0, resetTimer);
        }
        else
        {
            resetTimer = timeAfterInteractionUntilStopsWorking;
        }
    }

    public virtual void StartInteracting()
    {
        ShipState.Instance.stopPlayerMovementRequests.Add(this);
        PlayerIsInteracting = true;
    }

    public virtual void StopInteracting()
    {
        ShipState.Instance.stopPlayerMovementRequests.Remove(this);
        PlayerIsInteracting = false;
    }

    public virtual KeyCode GetInteractKey()
    {
        return ShipState.Instance.input.interact;
    }
}
