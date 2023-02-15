using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrottleLeverUI : MonoBehaviour
{
    [SerializeField]
    Slider slider = null;

    [SerializeField]
    Text text = null;

    private float value = 0f;

    public float Value
    {
        get => value;
        set
        {
            float newValue = value;

            if (newValue > 1) newValue = 1;
            if (newValue < 0) newValue = 0;

            slider.value = Value;

            this.value = newValue;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //slider = GetComponentInChildren<Slider>();

        //text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Value.ToString("0%");
    }

    public void SliderUpdateValue()
    {
        Value = slider.value;
    }
}
