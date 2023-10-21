using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerInputMap
{
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
}
