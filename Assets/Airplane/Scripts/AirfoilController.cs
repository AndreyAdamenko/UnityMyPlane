using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirfoilController : MonoBehaviour
{
    [SerializeField]
    float maxAngle = 10;

    [SerializeField]
    public AirfoilType type = AirfoilType.horizontalFoil;

    Quaternion startRotation = Quaternion.identity;

    string axisName = "";

    int inversion = 1;

    Vector3 rotationAxis = Vector3.zero;

    public AirPlane airPlane = null;

    private float value = 0f;

    public float Value { 
        get
        {
            return value;
        }

        set
        {
            this.value = value;
            if (value > 1f) this.value = 1f;
            if (value < -1f) this.value = -1f; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.localRotation;

        switch (type)
        {
            case AirfoilType.horizontalFoil:
                axisName = "Vertical";
                inversion = -1;
                rotationAxis = Vector3.right;
                break;

            case AirfoilType.aileronRight:
                axisName = "Horizontal";
                inversion = 1;
                rotationAxis = Vector3.right;
                break;

            case AirfoilType.aleronLeft:
                axisName = "Horizontal";
                inversion = -1;
                rotationAxis = Vector3.right;
                break;

            case AirfoilType.rudder:
                axisName = "Rudder";
                inversion = -1;
                rotationAxis = Vector3.up;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (airPlane != null && airPlane.isActive)
        {
            Value = CPInput.GetAxis(axisName);
        }

        transform.localRotation = Quaternion.Euler((rotationAxis.x * maxAngle * Value * inversion) + startRotation.eulerAngles.x, (rotationAxis.y * maxAngle * Value * inversion) + startRotation.eulerAngles.y, (rotationAxis.z * maxAngle * Value * inversion) + startRotation.eulerAngles.z);
    }
}
