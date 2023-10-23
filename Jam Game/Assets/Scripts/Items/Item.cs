using UnityEngine;

public abstract class Item : ScriptableObject
{
    public Sprite sprite;

    public abstract void Use();
}
