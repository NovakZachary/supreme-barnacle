using UnityEngine;

public class Hull : InteractableSector
{
    [Header("Dependencies")]
    [SerializeField] private WaterPuddleArea waterPuddleArea;
    
    [Header("Configuration")]
    [SerializeField] private float floodTime = 10f;
    [SerializeField] private int maxWaterAreasSpawned = 3;

    private int floodCount = 0;
    private float timeUntilNextFlood;

    private void Start()
    {
        timeUntilNextFlood = floodTime;
    }

    protected override void Update()
    {
        base.Update();
        timeUntilNextFlood -= Time.deltaTime;
        
        if (ShipComponent.IsBroken && timeUntilNextFlood < 0 && floodCount < maxWaterAreasSpawned)
        {
            Instantiate(waterPuddleArea, collider.RandomPointWithinColliderArea(), Quaternion.identity);
            timeUntilNextFlood = floodTime;
            floodCount++;
        }
    }
}
