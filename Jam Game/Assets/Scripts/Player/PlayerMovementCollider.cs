using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCollider : MonoBehaviour
{
    public PlayerMovement playerMovement;

    // Easiest place to implement, but not a great idea
    private HashSet<WineBarrel> wineBarrels = new();

    private void Update()
    {
        wineBarrels.RemoveWhere(barrel => !barrel);
        if (wineBarrels.Count > 0)
        {
            ShipState.Instance.playerDrunkeness = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out WineBarrel wineBarrel))
        {
            wineBarrels.Add(wineBarrel);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out WineBarrel wineBarrel))
        {
            wineBarrels.Remove(wineBarrel);
        }
    }

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
