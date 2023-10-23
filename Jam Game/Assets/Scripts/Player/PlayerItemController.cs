using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject itemSlot;
    [SerializeField] private SpriteRenderer sprite;

    public Item heldItem;

    private void Update()
    {
        itemSlot.SetActive(heldItem != null);
        sprite.sprite = heldItem == null ? null : heldItem.sprite;

        if (heldItem && Input.GetKey(ShipState.Instance.input.interact))
        {
            heldItem.Use();
            heldItem = null;
        }
    }
}
