using UnityEngine;

public class UIStateSetter : MonoBehaviour
{
    [SerializeField] private UIInterface uiInterface;

    private void Update()
    {
        uiInterface.distance = ShipState.Instance.distanceTraveled;
        uiInterface.health = ShipState.Instance.shipHealth;
        uiInterface.updateMaxHealth(ShipState.Instance.shipMaxHealth);
        uiInterface.isNight = ShipState.Instance.isNight;
        uiInterface.shipAngle = ShipState.Instance.shipAngle;
    }
}
