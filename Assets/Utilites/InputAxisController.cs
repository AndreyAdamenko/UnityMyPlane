using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAxisController
{
    float lastAxisValue = 0f;

    float firstValue = 0f;

    float secondValue = 0f;

    public InputAxisController(float firstValue, float secondValue)
    {
        this.firstValue = firstValue;
        this.secondValue = secondValue;
    }

    public bool IsValueChanged(float value)
    {
        bool result = false;
        
        if (lastAxisValue != value)
        {
            if (value == firstValue) result = true;
            if (value == secondValue) result = true;
        }

        lastAxisValue = value;

        return result;
    }
}
