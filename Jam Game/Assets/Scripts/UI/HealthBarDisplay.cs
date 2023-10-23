using UnityEngine;
using UnityEngine.UI;

public class HealthBarDisplay : MonoBehaviour
{
    public Slider healthBar;
    public ShipComponent component;

    private void Update()
    {
        Render();
    }

    private void Render()
    {
        healthBar.value = Mathf.Clamp01(component.health / component.maxHealth);
    }
}
