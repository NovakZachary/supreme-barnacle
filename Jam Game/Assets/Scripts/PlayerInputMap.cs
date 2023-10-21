using System;
using UnityEngine;

[Serializable]
public class PlayerInputMap
{
    public KeyCode sprint = KeyCode.LeftShift;

    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
}
