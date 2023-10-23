using UnityEngine;

[RequireComponent(typeof(ShipComponent))]
public class InteractableSector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Item interactionItem;
    [SerializeField] protected InteractableCollider collider;

    [Header("Configuration")] 
    [SerializeField] private int priority;
    [SerializeField] private float timeAfterInteractionUntilStopsWorking;
    [SerializeField] private bool stopsPlayerMovement = true;
    [SerializeField] private bool removesItemWhenOutOfRange = true;

    private PlayerInteract playerInteract;
    public int Priority => priority;
    public bool PlayerIsInteracting => playerInteract != null && playerInteract.CurrentInteractable == this;
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
        if (collider.IsPlayerColliding(out playerInteract))
        {
            playerInteract.CollidingWithInteractable(this);
        }
        else if (playerInteract != null)
        {
            playerInteract.StopCollidingWithInteractable(this);
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
        //PlayerIsInteracting = true;
        Player.Instance.items.heldItem = interactionItem;

        if (stopsPlayerMovement)
        {
            ShipState.Instance.stopPlayerMovementRequests.Add(this);
        }
    }

    public virtual void StopInteracting()
    {
        ShipState.Instance.stopPlayerMovementRequests.Remove(this);

        if (removesItemWhenOutOfRange)
        {
            Player.Instance.items.heldItem = null;
        }
    }

    public virtual KeyCode GetInteractKey()
    {
        return ShipState.Instance.input.interact;
    }
}
