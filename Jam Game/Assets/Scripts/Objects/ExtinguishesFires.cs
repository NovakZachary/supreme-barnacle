using UnityEngine;

public class ExtinguishesFires : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Fire fire))
        {
            Destroy(fire.gameObject);
        }
    }
}
