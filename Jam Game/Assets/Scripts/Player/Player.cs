using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    [Header("Dependencies")]
    public PlayerMovement movement;
    public PlayerItemController items;
}
