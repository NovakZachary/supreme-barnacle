using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableSector : InteractableSector
{
    [SerializeField] private float repairHealthPerSecond = 10f;

    protected override void Update()
    {
        base.Update();
        if (PlayerIsInteracting)
        {
            var healAmount = repairHealthPerSecond - (repairHealthPerSecond * ShipState.Instance.playerDrunkeness);
            ShipComponent.health += healAmount * Time.deltaTime;
        }
    }

    public override void StartInteracting()
    {
        base.StartInteracting();
        ShipComponent.health += repairHealthPerSecond - (repairHealthPerSecond * ShipState.Instance.playerDrunkeness);
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
    }

    public override KeyCode GetInteractKey()
    {
        return ShipState.Instance.input.repair;
    }
}
