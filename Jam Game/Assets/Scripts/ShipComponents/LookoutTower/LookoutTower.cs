using System.Collections.Generic;
using UnityEngine;

public class LookoutTower : InteractableSector
{
    public readonly HashSet<object> LookoutRequesters = new HashSet<object>();
    [SerializeField] private LookoutCameras lookoutCameras;

    [SerializeField] private int enabledPriority = 100;
    [SerializeField] private int disabledPriority = -1;
    
    private void Start()
    {
        DisableLookoutCameras();
    }

    private void Update()
    {
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
        lookoutCameras.EnhancedLookoutCamera.Priority = enabledPriority;
    }

    private void DisableLookoutCameras()
    {
        lookoutCameras.EnhancedLookoutCamera.Priority = disabledPriority;
    }
}
