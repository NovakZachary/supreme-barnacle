using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineBarrel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        ShipState.Instance.playerDrunkeness = 1f;
    }
}
