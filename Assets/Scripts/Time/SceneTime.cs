using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTime : MonoBehaviour
{
    public static DateTime time = new DateTime();

    public static float timeSpeed = 100f;

    public float curTimeMinutes
    {
        get
        {
            return time.Minute + (time.Hour * 60);
        }

        set
        {
            DateTime newTime = new DateTime().AddMinutes(value);

            time = newTime;
        }
    }

    public int startTimeSeconds = 43200;

    public bool timeIsRuning = false;

    private void Start()
    {
        time = time.AddSeconds(startTimeSeconds);
    }

    private void LateUpdate()
    {
         if (timeIsRuning) time = time.AddSeconds(timeSpeed * Time.deltaTime);
    }

    public static int GetDaySeconds()
    {
        TimeSpan tSpan = time.TimeOfDay;

        return (tSpan.Hours * 3600) + (tSpan.Minutes * 60) + tSpan.Seconds;
    }
}
