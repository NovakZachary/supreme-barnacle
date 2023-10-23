using UnityEngine;

public class Sail : MonoBehaviour
{
    [SerializeField] private GameObject raised;
    [SerializeField] private GameObject lowered;
    
    private void Update()
    {
        if (ShipState.Instance.MastLowered)
        {
            lowered.SetActive(true);
            raised.SetActive(false);
        }
        else
        {
            lowered.SetActive(false);
            raised.SetActive(true);
        }

        ShipState.Instance.distanceTraveled += ShipState.Instance.shipSpeed.y * Time.deltaTime;
    }
}
