using UnityEngine;
using Random = UnityEngine.Random;

public class Railing : MonoBehaviour
{
    public void DamageShip(float damage, Vector3 collisionPoint)
    {
        var item = Random.Range(0, ShipState.Instance.shipComponents.Count);
        var i = 0;
        
        foreach (var instanceShipComponent in ShipState.Instance.shipComponents)
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
