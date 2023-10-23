using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private HashSet<InteractableSector> touchedInteractables = new();

    private InteractableSector highestPriorityInteractable;
    private bool isInteracting;

    public InteractableSector activeInteractable => isInteracting ? highestPriorityInteractable : null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out InteractableSector interactable))
        {
            touchedInteractables.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out InteractableSector interactable))
        {
            touchedInteractables.Remove(interactable);
        }
    }

    private void Update()
    {
        UpdateHighestPriorityInteractable();
        
        if (highestPriorityInteractable && Input.GetKeyDown(highestPriorityInteractable.GetInteractKey()))
        {
            Debug.Log($"Player has interacted with {highestPriorityInteractable.name}");
            
            if (!isInteracting)
            {
                Debug.Log($"Interacting");
                highestPriorityInteractable.StartInteracting();
                isInteracting = true;
            }
            else
            {
                Debug.Log($"Stop Interacting");
                highestPriorityInteractable.StopInteracting();
                isInteracting = false;
            }
        }

        touchedInteractables.Dump();
        highestPriorityInteractable.Dump();
        isInteracting.Dump();
    }

    private void UpdateHighestPriorityInteractable()
    {
        InteractableSector highestInteractable = null;
        var highestPriority = int.MinValue;
        foreach (var interactable in touchedInteractables)
        {
            if (interactable.Priority > highestPriority)
            {
                highestInteractable = interactable;
                highestPriority = interactable.Priority;
            }
        }

        SetHighestPriorityInteractable(highestInteractable);
    }

    private void SetHighestPriorityInteractable(InteractableSector interactable)
    {
        if (highestPriorityInteractable == interactable)
        {
            return;
        }

        if (highestPriorityInteractable && isInteracting)
        {
            highestPriorityInteractable.StopInteracting();
            isInteracting = false;
        }

        highestPriorityInteractable = interactable;
    }
}
