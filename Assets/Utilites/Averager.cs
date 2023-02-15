using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

public class Averager
{
    List<float> consumptions = null;

    float result = 0;

    public Averager(int bufferSize)
    {
        consumptions = new List<float>(bufferSize);
    }

    public float GetValue(float newValue)
    {
        consumptions.Add(newValue);

        if (consumptions.Count == consumptions.Capacity)
        {
            result = consumptions.Sum() / 60f;

            consumptions.Clear();
        }

        return result;
    }
}
