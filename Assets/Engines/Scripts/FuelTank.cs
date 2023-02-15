using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FuelTank : MonoBehaviour
{
    public float value = 100;

    public float capacity = 500;

    public float consumption = 0;

    private float lastValue = 0;

    Color startColor = Color.green;

    Averager consumptionAvergaer = new Averager(60);

    private void LateUpdate()
    {
        if (value > capacity) value = capacity;

        float curConsumption = (value - lastValue) / Time.deltaTime;

        lastValue = value;

        consumption = consumptionAvergaer.GetValue(curConsumption);
    }

    public bool GetFuel(float count)
    {
        if (value > 0 && count < value)
        {
            value -= count;
            return true;
        }

        return false;
    }

}
