using UnityEngine;

[CreateAssetMenu(menuName = "Project/Items/WaterBucket", fileName = "WaterBucketItem", order = 0)]
public class WaterBucketItem : Item
{
    [SerializeField] private WaterBucketProjectile projectilePrefab;

    public override void OnDrop()
    {
        var mouseScreenPosition = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

        var plane = new Plane(Vector3.forward, Player.Instance.transform.position);
        if (!plane.Raycast(ray, out var distance))
        {
            return;
        }

        var mouseWorldPosition = ray.GetPoint(distance);
        var direction = (mouseWorldPosition - Player.Instance.transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x);

        Instantiate(projectilePrefab, Player.Instance.transform.position, Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg));
    }
}
