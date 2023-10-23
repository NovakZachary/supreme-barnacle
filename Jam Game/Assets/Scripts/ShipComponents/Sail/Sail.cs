using UnityEngine;

public class Sail : MonoBehaviour
{
    [SerializeField] private GameObject raised;
    [SerializeField] private GameObject middle;
    [SerializeField] private GameObject lowered;
    
    private void Update()
    {
        if (ShipState.Instance.MastLowered)
        {
            lowered.SetActive(true);
            middle.SetActive(false);
            raised.SetActive(false);
        }
        else if(ShipState.Instance.MastRaised)
        {
            lowered.SetActive(false);
            middle.SetActive(false);
            raised.SetActive(true);
        }
        else
        {
            lowered.SetActive(false);
            middle.SetActive(true);
            raised.SetActive(false);
        }

        ShipState.Instance.distanceTraveled += ShipState.Instance.shipVelocity.y * Time.deltaTime;
    }
}
