using UnityEngine;

public class RepairableSector : InteractableSector
{
    [SerializeField] private float repairHealthPerSecond = 10f;

    protected virtual void Update()
    {
        if (Player.Instance.interaction.activeInteractable == this)
        {
            var healAmount = Mathf.Lerp(repairHealthPerSecond, 0, ShipState.Instance.playerDrunkeness);
            ShipComponent.health += healAmount * Time.deltaTime;
        }
    }

    public override KeyCode GetInteractKey()
    {
        return ShipState.Instance.input.repair;
    }
}
