using System.Collections.Generic;
using UnityEngine;

public class ShipComponentHealthbarDisplay : MonoBehaviour
{
    public HealthBarDisplay healthBarPrefab;

    private Dictionary<ShipComponent, HealthBarDisplay> healthBars = new();

    private void Start()
    {
        foreach (var component in ShipState.Instance.ShipComponents)
        {
            AddHealthBar(component);
        }

        ShipState.Instance.ShipComponentAdded += AddHealthBar;
        ShipState.Instance.ShipComponentRemoved += RemoveHealthBar;
    }

    private void AddHealthBar(ShipComponent component)
    {
        var display = Instantiate(healthBarPrefab, transform);
        display.component = component;

        healthBars.Add(component, display);
    }

    private void RemoveHealthBar(ShipComponent component)
    {
        if (healthBars.TryGetValue(component, out var display))
        {
            if (display)
            {
                Destroy(display.gameObject);
            }

            healthBars.Remove(component);
        }
    }
}
