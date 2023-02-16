using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlane : MonoBehaviour
{
    public bool isActive = false;

    public List<AirfoilController> ailerons = new List<AirfoilController>();

    public List<AirfoilController> elevators = new List<AirfoilController>();

    public List<AirfoilController> rudders = new List<AirfoilController>();

    public List<EngineController> engines = new List<EngineController>();

    private Rigidbody _rb;


    public float roll = 0f;

    public float rollSpeed = 0f;

    public float pitch = 0f;

    public float pitchSpeed = 0f;

    public float frontSpeed = 0f;

    private float _lastRoll = 0f;
    private float _lastPitch = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        AirfoilController[] airfoilControllers = transform.GetComponentsInChildren<AirfoilController>();

        foreach (AirfoilController controller in airfoilControllers)
        {
            controller.airPlane = this;

            if (controller.type == AirfoilType.aileronRight || controller.type == AirfoilType.aleronLeft)
            {
                ailerons.Add(controller);
            }
            else if (controller.type == AirfoilType.horizontalFoil)
            {
                elevators.Add(controller);
            }
            else if (controller.type == AirfoilType.rudder)
            {
                rudders.Add(controller);
            }
        }

        EngineController[] engineControllers = transform.GetComponentsInChildren<EngineController>();

        foreach (EngineController controller in engineControllers)
        {
            controller.airPlane = this;

            engines.Add(controller);
        }
    }

    private void FixedUpdate()
    {
        frontSpeed = transform.InverseTransformDirection(_rb.velocity).z;

        Vector3 flatForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        Vector3 flatLeft = Vector3.Cross(flatForward, Vector3.up).normalized;

        roll = AngleUtil.AngleOffAroundAxis(flatLeft, -transform.right, flatForward);

        pitch = AngleUtil.AngleOffAroundAxis(transform.forward, flatForward, -flatLeft);


        rollSpeed = roll - _lastRoll;
        _lastRoll = roll;

        pitchSpeed = pitch - _lastPitch;
        _lastPitch = pitch;
    }

    public float GetVelocityVerticalAngle()
    {
        Vector3 flatForward = new Vector3(_rb.velocity.x, 0, _rb.velocity.z).normalized;

        Vector3 flatLeft = Vector3.Cross(flatForward, Vector3.up).normalized;

        var horizontalAngle = AngleUtil.AngleOffAroundAxis(flatLeft, -transform.right, flatForward);

        var verticalAngle = AngleUtil.AngleOffAroundAxis(transform.forward, flatForward, -flatLeft);

        return verticalAngle;
    }

    public void SetAilerons(float value)
    {
        foreach (AirfoilController airfoil in ailerons)
        {
            airfoil.Value = value;
        }
    }

    public void SetElevators(float value)
    {
        foreach (AirfoilController airfoil in elevators)
        {
            airfoil.Value = value;
        }
    }

    public void SetRudders(float value)
    {
        foreach (AirfoilController airfoil in rudders)
        {
            airfoil.Value = value;
        }
    }

    public void SetThrottle(float value)
    {
        foreach (EngineController engine in engines)
        {
            engine.CurTraction = value;
        }
    }
}
