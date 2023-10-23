using UnityEngine;
using UnityEngine.Serialization;

public class PlayerItemController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject itemSlot;
    [FormerlySerializedAs("sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Item heldItem;

    private Item previousHeldItem;

    private void Update()
    {
        if (previousHeldItem != heldItem && heldItem)
        {
            heldItem.OnPickup();
        }

        if (previousHeldItem != heldItem && previousHeldItem)
        {
            previousHeldItem.OnDrop();
        }

        previousHeldItem = heldItem;

        itemSlot.SetActive(heldItem != null);
        spriteRenderer.sprite = heldItem == null ? null : heldItem.sprite;

        if (heldItem)
        {
            spriteRenderer.transform.localPosition = heldItem.position;
            spriteRenderer.transform.localScale = heldItem.scale;

            if (Input.GetKeyDown(ShipState.Instance.input.interact))
            {
                heldItem = null;
            }
        }
    }
}
