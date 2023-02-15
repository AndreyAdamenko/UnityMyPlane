using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    Light light = null;

    public float maxIntensity = 1f;

    public float speed = 0.02f;

    public float timeOffset = 0f;

    float curIntensity = 0;


    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();

        curIntensity = maxIntensity;
    }

    private void FixedUpdate()
    {
        float sin = Mathf.Sin((Time.time + timeOffset) * speed);
        
        curIntensity = ((sin + 1f) / 2f) * maxIntensity;

        light.intensity = curIntensity;
    }
}
