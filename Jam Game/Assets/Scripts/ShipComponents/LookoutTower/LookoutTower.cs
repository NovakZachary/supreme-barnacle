using System.Collections.Generic;
using UnityEngine;

public class LookoutTower : InteractableSector
{
    public readonly HashSet<object> LookoutRequesters = new HashSet<object>();
    [SerializeField] private LookoutCameras lookoutCameras;

    [SerializeField] private int maxPriority = 100;
    [SerializeField] private int minPriority = -1;
    
    private void Start()
    {
        DisableLookoutCameras();
    }

    protected override void Update()
    {
        base.Update();
        if (LookoutRequesters.Count > 0)
        {
            EnableLookoutCamera();
        }
        else
        {
            DisableLookoutCameras();
        }
    }

    public override void StartInteracting()
    {
        base.StartInteracting();
        LookoutRequesters.Add(this);
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
        LookoutRequesters.Remove(this);
    }

    private void EnableLookoutCamera()
    {
        if (HasReset)
        {
            lookoutCameras.BasicLookoutCamera.Priority = maxPriority;
            lookoutCameras.EnhancedLookoutCamera.Priority = minPriority;
        }
        else
        {
            lookoutCameras.BasicLookoutCamera.Priority = minPriority;
            lookoutCameras.EnhancedLookoutCamera.Priority = maxPriority;
        }
    }

    private void DisableLookoutCameras()
    {
        lookoutCameras.BasicLookoutCamera.Priority = minPriority;
        lookoutCameras.EnhancedLookoutCamera.Priority = minPriority;
    }
}
