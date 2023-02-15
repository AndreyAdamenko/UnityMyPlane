using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakController : MonoBehaviour
{
    [SerializeField]
    WheelsController wheels = null;

    [SerializeField]
    float brakeTorque = 300;

    public BreaksButtonController buttonController = null;


    public bool state = false;

    InputAxisController breakAxis = new InputAxisController(1, -1);

    AirPlane airPlane = null;

    // Start is called before the first frame update
    void Start()
    {
        airPlane = GetComponent<AirPlane>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!airPlane.isActive) return;
        
        float breakAxisValue = Input.GetAxis("Breaks");

        if (breakAxis.IsValueChanged(breakAxisValue))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        foreach (AxleInfo axleInfo in wheels.axleInfos)
        {
            if (axleInfo.haveBrakes)
            {

                if (state)
                {

                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.brakeTorque = 0;

                    axleInfo.leftWheel.motorTorque = 0.00001f;
                    axleInfo.rightWheel.motorTorque = 0.00001f;

                    state = false;

                    buttonController.Free();
                }
                else
                {
                    axleInfo.leftWheel.brakeTorque = brakeTorque;
                    axleInfo.rightWheel.brakeTorque = brakeTorque;

                    state = true;

                    buttonController.Press();
                }
            }
        }
    }
}
