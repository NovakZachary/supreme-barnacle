using UnityEngine;

[RequireComponent(typeof(ShipComponent))]
public class InteractableSector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Item interactionItem;
    [SerializeField] protected InteractableCollider collider;

    [Header("Configuration")] 
    [SerializeField] private int priority;
    [SerializeField] private bool stopsPlayerMovement = true;
    [SerializeField] private bool removesItemWhenOutOfRange = true;

    public int Priority => priority;

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

    public virtual void StartInteracting()
    {
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
