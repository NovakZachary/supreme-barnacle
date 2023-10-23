using UnityEngine;
using UnityEngine.Serialization;

public class WaterPuddle : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public WaterPuddleArea area;

    public float activeDuration = 15;
    public float fadeDuration = 2;

    public float activeOpacity = 0.75f;

    private float timer;

    private void Update()
    {
        var totalDuration = activeDuration + fadeDuration;

        timer += Time.deltaTime;

        var color = spriteRenderer.color;
        color.a = timer < activeDuration ? activeOpacity : Mathf.Lerp(activeOpacity, 0, (timer - activeDuration) / fadeDuration);
        spriteRenderer.color = color;

        area.enabled = timer < activeDuration;

        if (timer > totalDuration)
        {
            Destroy(gameObject);
        }
    }
}
