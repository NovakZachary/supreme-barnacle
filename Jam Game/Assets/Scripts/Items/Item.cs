using UnityEngine;

[CreateAssetMenu(menuName = "Project/Item/Basic Item", fileName = "Item", order = 0)]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public Vector3 position = Vector3.zero;
    public Vector3 scale = Vector3.one;

    public virtual void OnPickup() {}
    public virtual void OnDrop() {}
}
