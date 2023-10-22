using System.Collections;
using System.Collections.Generic;
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
    }
}
