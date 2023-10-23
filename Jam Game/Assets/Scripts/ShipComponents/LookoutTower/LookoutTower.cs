using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookoutTower : InteractableSector
{
    [SerializeField] private LookoutCameras lookoutCameras;

    [SerializeField] private int maxPriority = 100;
    [SerializeField] private int minPriority = -1;
    
    private void Start()
    {
        DisableLookoutCameras();
    }

    public override void StartInteracting()
    {
        base.StartInteracting();
        EnableLookoutCamera();
    }
    
    public override void StopInteracting()
    {
        base.StopInteracting();
        DisableLookoutCameras();
    }
    
    public void EnableLookoutCamera()
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

    public void DisableLookoutCameras()
    {
        lookoutCameras.BasicLookoutCamera.Priority = minPriority;
        lookoutCameras.EnhancedLookoutCamera.Priority = minPriority;
    }
}
