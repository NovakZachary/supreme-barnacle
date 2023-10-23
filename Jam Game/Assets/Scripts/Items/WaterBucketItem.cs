using UnityEngine;

[CreateAssetMenu(menuName = "Project/Items/WaterBucket", fileName = "WaterBucketItem", order = 0)]
public class WaterBucketItem : Item
{
    public override void OnPickup()
    {
        Debug.Log("WaterBucketItem Up");
    }

    public override void OnDrop()
    {
        Debug.Log("WaterBucketItem");
    }
}
