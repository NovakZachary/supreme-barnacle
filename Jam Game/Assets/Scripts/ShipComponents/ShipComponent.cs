using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    public string displayName = "Ship Component";

    public float health = 100;
    public float maxHealth = 100;
    public float damageTakenPerSecond = 1;

    [Tooltip("Whether the component contributes to ship health.")]
    public bool isStructural = true;

    private void OnEnable()
    {
        ShipState.Instance.shipComponents.Add(this);
    }

    private void OnDisable()
    {
        ShipState.Instance.shipComponents.Remove(this);
    }

    private void Awake()
    {
        health = maxHealth;
    }

    private void Update()
    {
        health -= damageTakenPerSecond * ShipState.Instance.shipDamageTakenMultiplier * Time.deltaTime;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
