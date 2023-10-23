using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LookoutCameras : MonoBehaviour
{
    [SerializeField] private ShipComponent shipComponent;
    
    public CinemachineVirtualCamera BasicLookoutCamera;
    public CinemachineVirtualCamera EnhancedLookoutCamera;
    
    public void EnableLookoutCamera()
    {
        if (shipComponent.IsBroken)
        {
            
        }
    }

    public void DisableLookoutCameras()
    {
        
    }
}
