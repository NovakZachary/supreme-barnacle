using UnityEngine;

public abstract class Item : ScriptableObject
{
    public Sprite sprite;

    public abstract void OnPickup();
    public abstract void OnDrop();
}
