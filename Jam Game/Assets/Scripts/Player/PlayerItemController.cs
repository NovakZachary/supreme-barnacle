using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject itemSlot;
    [SerializeField] private SpriteRenderer sprite;

    public Item heldItem;

    private Item previousHeldItem;

    private void Update()
    {
        if (previousHeldItem != heldItem && heldItem)
        {
            heldItem.OnPickup();
        }

        previousHeldItem = heldItem;

        itemSlot.SetActive(heldItem != null);
        sprite.sprite = heldItem == null ? null : heldItem.sprite;

        if (heldItem && Input.GetKey(ShipState.Instance.input.interact))
        {
            heldItem.OnDrop();
            heldItem = null;
        }
    }
}
