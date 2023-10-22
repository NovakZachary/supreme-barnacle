using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class SunLight : MonoBehaviour
{
    [FormerlySerializedAs("light")]
    public Light2D targetLight;

    public float[] brightnessAtHours = new float[24];

    private void Update()
    {
        var brightness1 = brightnessAtHours[ShipState.Instance.currentDayHour];
        var brightness2 = brightnessAtHours[(ShipState.Instance.currentDayHour + 1) % 24];
        var brightness = Mathf.Lerp(brightness1, brightness2, ShipState.Instance.currentDayTime % 1);

        targetLight.intensity = brightness;
    }
}
