using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    SceneTime sTime = null;

    Slider slider = null;

    // Start is called before the first frame update
    void Start()
    {
        sTime = GameObject.FindGameObjectsWithTag("Time")[0].GetComponent<SceneTime>();

        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTime()
    {
        float sliderValue = slider.value;

        sTime.curTimeMinutes = sliderValue;
    }
}
