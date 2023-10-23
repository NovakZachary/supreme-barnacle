using UnityEngine;
using Random = UnityEngine.Random;

public class Railing : MonoBehaviour
{
    public void DamageShip(float damage)
    {
        var item = Random.Range(0, ShipState.Instance.ShipComponents.Count);
        var i = 0;
        
        foreach (var instanceShipComponent in ShipState.Instance.ShipComponents)
        {
            if (i == item)
            {
                instanceShipComponent.health -= damage;
                Debug.Log($"Damaged ship part: { instanceShipComponent.displayName }");
                return;
            }
        }
    }
}
