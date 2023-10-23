using UnityEngine;

/// <summary>
/// Used because Unity doesn't handle recursive prefab references well.
/// </summary>
[CreateAssetMenu(menuName = "Project/FireData", fileName = "FireData", order = 0)]
public class FireData : ScriptableObject
{
    public Fire firePrefab;
}
