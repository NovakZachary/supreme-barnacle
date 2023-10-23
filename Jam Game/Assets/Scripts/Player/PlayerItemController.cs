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
    private int lastChangeFrame;

    private void Update()
    {
        if (heldItem != previousHeldItem && heldItem)
        {
            Debug.Log("Picked up");

            heldItem.OnPickup();
        }

        if (heldItem != previousHeldItem && previousHeldItem)
        {
            Debug.Log("Used");

            previousHeldItem.OnDrop();
        }

        if (previousHeldItem != heldItem)
        {
            previousHeldItem = heldItem;
            lastChangeFrame = Time.frameCount;
        }

        itemSlot.SetActive(heldItem != null);
        spriteRenderer.sprite = heldItem == null ? null : heldItem.sprite;
        if (heldItem)
        {
            spriteRenderer.transform.localPosition = heldItem.position;
            spriteRenderer.transform.localScale = heldItem.scale;
        }

        if (lastChangeFrame != Time.frameCount && Input.GetKeyDown(ShipState.Instance.input.interact))
        {
            heldItem = null;
        }
    }
}
