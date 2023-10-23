using UnityEngine;

public class DestroyRocksOnCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent<FloatingRocks>(out _))
        {
            Destroy(col.gameObject);
        }
    }
}
