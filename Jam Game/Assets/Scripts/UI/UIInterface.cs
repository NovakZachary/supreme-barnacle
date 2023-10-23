using UnityEngine.UI;
using UnityEngine;

public class UIInterface : MonoBehaviour
{
    [Header("Day Night Indicator")]
    public bool isNight;
    public Image DayNightIndicator;
    public Sprite dayImage;
    public Sprite nightImage;

    [Header("Event Indicator")]
    public bool isEvent;
    public GameObject eventIndicator;

    [Header("Health Bar")]
    public float maxHealth;
    public float health;
    public Slider bar;

    [Header("Tilt Indicator")]
    private float maxVal = 180;
    [Range(-90, 90)]
    public float shipAngle = 0;
    public Slider tiltBar;

    void Start()
    {
        updateMaxHealth(maxHealth);
        tiltBar.maxValue = maxVal;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNight) 
        {
            DayNightIndicator.sprite = nightImage;
        } else 
        {
            DayNightIndicator.sprite = dayImage;
        }

        if (isEvent) 
        {
            eventIndicator.SetActive(true);
        } else 
        {
            eventIndicator.SetActive(false);
        }

        health = Mathf.Clamp(health, 0, maxHealth);
        bar.value = health;
        setAngleUI();
    }

    void updateMaxHealth(float val)
    {
        maxHealth = val;
        bar.maxValue = val;
    }

    void setAngleUI()
    {
        float c = shipAngle + 90;
        tiltBar.value = c;
    }
}
