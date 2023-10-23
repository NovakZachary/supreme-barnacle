using UnityEngine;

[CreateAssetMenu(menuName = "Project/Items/WaterBucket", fileName = "WaterBucketItem", order = 0)]
public class WaterBucketItem : Item
{
    public override void Use()
    {
        Debug.Log("WaterBucketItem");
    }
}
