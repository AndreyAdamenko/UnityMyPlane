using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Second timer
/// </summary>
public class CustomTimer
{
    public float period = 0;

    private float lastTime = 0;
    
    public CustomTimer(float period, float lastTime)
    {
        this.period = period;

        this.lastTime = lastTime;
    }

    public bool isReady()
    {
        float curTime = Time.time;

        if (curTime > lastTime + period)
        {
            lastTime = curTime;

            return true;
        }

        return false;
    }

    public static float GetRealSeconds(float gameSeconds)
    {
        float period = (1f / SceneTime.timeSpeed) * gameSeconds;

        return period;
    }
}
