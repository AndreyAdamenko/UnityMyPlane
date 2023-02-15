using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    GameObject sun = null;

    Light light = null;

    CustomTimer timer = null;

    public static float degreesPerSecond = 0f;

    private float lightIntencity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        sun = gameObject;

        light = transform.GetComponentInChildren<Light>();

        float ColorTimerPeriod = (1f / SceneTime.timeSpeed) * 1000f;

        timer = new CustomTimer(CustomTimer.GetRealSeconds(100f), 0f);

        degreesPerSecond = (90f / 21600f);

        lightIntencity = light.intensity;


    }

    // Update is called once per frame
    void LateUpdate()
    {
        float seconds = SceneTime.GetDaySeconds();

        float sunAngle = degreesPerSecond * seconds;

        Quaternion newRotation = Quaternion.Euler(0, 0, sunAngle - 90);

        sun.transform.rotation = newRotation;

        if (sunAngle < 90f)
        {
            light.intensity = lightIntencity * (((sunAngle / 90f) * 4f) - 3f);
        }
        else if (sunAngle > 270f)
        {
            light.intensity = lightIntencity * (((1f - ((sunAngle - 270f) / 90f)) * 4f) - 3f);
        }
        else
        {
            light.intensity = lightIntencity;
        }
    }

    public static float GetSunAngle()
    {
        float sunAngle = SunController.degreesPerSecond * SceneTime.GetDaySeconds();

        return sunAngle;
    }
}
