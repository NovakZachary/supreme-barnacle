using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIInterface : MonoBehaviour
{
    [Header("Day Night Indicator")]
    public Image DayNightIndicator;
    public Sprite dayImage;
    public Sprite nightImage;

    [Header("Event Indicator")]
    public bool isEvent;
    public GameObject eventIndicator;

    [FormerlySerializedAs("bar")]
    [Header("Health Bar")]
    public Slider healthBar;

    [Header("Tilt Indicator")]
    [Range(-90, 90)]
    public float shipAngle = 0;
    public Slider tiltBar;

    [Header("Distance Tracker")]
    public float distance = 0;
    [FormerlySerializedAs("text")]
    public TMP_Text distanceText;

    [Header("Speed Tracker")]
    public float speed;
    public TMP_Text speedText;

    private void Update()
    {
        // Day/night indicator
        if (ShipState.Instance.isNight)
        {
            DayNightIndicator.sprite = nightImage;
        }
        else
        {
            DayNightIndicator.sprite = dayImage;
        }

        // Event indicator
        if (isEvent)
        {
            eventIndicator.SetActive(true);
        }
        else
        {
            eventIndicator.SetActive(false);
        }

        distance = ShipState.Instance.distanceTraveled;
        distanceText.text = $"{distance:F1} m";

        healthBar.value = ShipState.Instance.shipIntegrity;

        speed = ShipState.Instance.shipSpeed.y;
        speedText.text = $"{speed:F1} m/s";

        shipAngle = ShipState.Instance.shipAngle;
        tiltBar.value = (shipAngle + 90) / 180;
    }
}
