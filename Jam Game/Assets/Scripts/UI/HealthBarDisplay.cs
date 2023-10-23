using UnityEngine;
using UnityEngine.UI;

public class HealthBarDisplay : MonoBehaviour
{
    public Slider healthBar;
    public Vector2 offset;
    public ShipComponent component;

    private void Update()
    {
        Render();
    }

    private void Render()
    {
        var camera = Camera.main;
        var worldPosition = component.transform.position + (Vector3)offset;

        var screenPosition = camera.WorldToScreenPoint(worldPosition);
        transform.position = screenPosition;

        healthBar.value = Mathf.Clamp01(component.health / component.maxHealth);
    }
}
