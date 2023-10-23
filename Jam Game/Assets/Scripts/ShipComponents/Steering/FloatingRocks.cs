using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FloatingRocks : MonoBehaviour
{
    [Header("Configuration")]
    public bool IsMatchingBoatSpeed = true;
    public float damageOnCollision;

    [Header("Velocity relative to world")]
    [SerializeField] private float speedUnitsPerSecond = 1f;
    [SerializeField] private Vector3 velocityDirection;
    [SerializeField] private Vector3 additionalVelocity = Vector3.zero;
    
    [Header("Rotate around pivot")]
    [SerializeField] private float rotationDegreesPerSecond = 1f;
    [SerializeField] private Vector3 pivotPosition;

    [Header("Destroy self")]
    [SerializeField] private bool destroyWhenFarFromPivot = true;
    [SerializeField] private float distanceFromPivotUntilDestroyed = 10f;
    
    private void Update()
    {
        if (IsMatchingBoatSpeed)
        {
            additionalVelocity = new Vector3(
                    -ShipState.Instance.shipSpeed.x, //Ship turning left -> rock moves right. Vice versa. 
                    additionalVelocity.y,
                    additionalVelocity.z
                );
        }
        
        transform.RotateAround(pivotPosition, Vector3.forward, rotationDegreesPerSecond * Time.deltaTime);
        
        velocityDirection = Vector3.ClampMagnitude(velocityDirection, 1);
        transform.position += (velocityDirection * speedUnitsPerSecond + additionalVelocity) * Time.deltaTime;

        if (destroyWhenFarFromPivot && Vector3.Distance(transform.position, pivotPosition) > distanceFromPivotUntilDestroyed)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent<Railing>(out var railing))
        {
            railing.DamageShip(damageOnCollision, col.GetContact(0).point);
        }
    }
}
