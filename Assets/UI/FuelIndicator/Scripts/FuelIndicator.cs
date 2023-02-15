using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelIndicator : MonoBehaviour
{
    public FuelTank tank = null;

    [SerializeField]
    Color fullColor = Color.green;

    [SerializeField]
    Color emptyColor = Color.red;

    [SerializeField]
    float dangerLevel = 0.3f;

    Text txtFuelConsumption = null;

    Text txtFuelValue = null;

    RectTransform level = null;

    Image levelImage = null;

    Image backgroundImage = null;

    // Start is called before the first frame update
    void Start()
    {
        Transform levelTransform = transform.Find("Mask/Level");

        level = levelTransform.GetComponent<RectTransform>();

        levelImage = levelTransform.GetComponent<Image>();

        backgroundImage = transform.Find("Mask/Background").GetComponent<Image>();

        txtFuelConsumption = transform.Find("ConsumptionTxt").GetComponent<Text>();

        txtFuelValue = transform.Find("ValueTxt").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float levelValue = tank.value / tank.capacity;

        level.localPosition = new Vector2(0, -level.rect.height + (level.rect.height * levelValue));

        if (levelValue >= dangerLevel)
        {
            levelImage.color = fullColor;
            backgroundImage.color = fullColor;
            //txtFuelValue.color = Color.black;
        }
        else
        {
            levelImage.color = emptyColor;
            backgroundImage.color = emptyColor;
            //txtFuelValue.color = emptyColor;
        }

        if (tank.consumption < -0.001f)
        {
            txtFuelConsumption.enabled = true;

            txtFuelConsumption.text = tank.consumption.ToString("0.00") + "L/s";
        }
        else if (tank.consumption > 0.001f)
        {
            txtFuelConsumption.enabled = true;

            txtFuelConsumption.text = "+" + tank.consumption.ToString("0.00") + "L/s";
        }
        else
        {
            txtFuelConsumption.enabled = false;
        }

        txtFuelValue.text = tank.value.ToString("0.00") + "L";
    }
}
