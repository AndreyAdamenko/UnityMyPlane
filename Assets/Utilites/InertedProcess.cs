using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertedProcess
{

    private float min = 0f;

    private float max = 0f;

    public float curValue = 0f;



    public InertedProcess(float min, float max)
    {
        this.min = min;

        this.max = max;
    }

    public void CalculateAccumulatedValue(float selectedValue, float speed)
    {
        
        float direction = 0f;
        
        if (selectedValue > curValue)
        {
            direction = 1;
        }
        else if (selectedValue < curValue)
        {
            direction = -1;
        }



        if (direction > 0 && curValue < max)
        {
            curValue += direction * speed;
            if (curValue > max) curValue = max;
        }

        if (direction < 0 && curValue > min)
        {
            curValue += direction * speed;
            if (curValue < min) curValue = min;
        }
    }
}
