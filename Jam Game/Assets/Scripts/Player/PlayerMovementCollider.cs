using UnityEngine;

public class PlayerMovementCollider : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IceArea iceArea))
        {
            playerMovement.iceAreas.Add(iceArea);
        }

        if (other.TryGetComponent(out WaterPuddleArea waterPuddleArea))
        {
            playerMovement.waterPuddleAreas.Add(waterPuddleArea);
        }

        if (other.TryGetComponent(out SlowArea slowArea))
        {
            playerMovement.slowAreas.Add(slowArea);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IceArea iceArea))
        {
            playerMovement.iceAreas.Remove(iceArea);
        }

        if (other.TryGetComponent(out WaterPuddleArea waterPuddleArea))
        {
            playerMovement.waterPuddleAreas.Remove(waterPuddleArea);
        }

        if (other.TryGetComponent(out SlowArea slowArea))
        {
            playerMovement.slowAreas.Remove(slowArea);
        }
    }
}
