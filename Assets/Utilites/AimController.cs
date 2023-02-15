using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using HomeDev.Planes;

public class AimController : MonoBehaviour
{
    Rigidbody _rb;

    AirPlane _airPlane;

    float lastAltitude = 0;

    float verticalSpeed = 0;

    float frontSpeed = 0;

    float lastFrontSpeed = 0;

    float frontAcceleration = 0;


    public AimIndicator aimIndicator = null;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _airPlane = GetComponent<AirPlane>();

        lastAltitude = transform.position.y;

        //aimIndicator = AimIndicator.instance;
    }

    private void Calculate()
    {
        verticalSpeed = (transform.position.y - lastAltitude) * (1 / Time.fixedDeltaTime);

        lastAltitude = transform.position.y;

        frontSpeed = transform.InverseTransformDirection(_rb.velocity).z;

        frontAcceleration = (frontSpeed - lastFrontSpeed) * (1 / Time.fixedDeltaTime);

        lastFrontSpeed = frontSpeed;
    }

    private void FixedUpdate()
    {
        Calculate();

        if (aimIndicator == null) return;

        SetAim();
    }

    private void SetAim()
    {
        // levelPitchAngle

        Vector3 noseFlatForwardVector = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        Vector3 noseLeft = Vector3.Cross(noseFlatForwardVector, Vector3.up).normalized;

        //float nosePitchAngle = AngleUtil.AngleOffAroundAxis(transform.forward, noseFlatForwardVector, noseLeft);

        aimIndicator.levelPitchAngle = _airPlane.pitch;

        // levelRollAngle

        //float noseRollAngle = AngleUtil.AngleOffAroundAxis(transform.right, -noseLeft, noseFlatForwardVector);

        aimIndicator.levelRollAngle = _airPlane.roll;

        // pointHorizontalAngle

        if (_rb.velocity.magnitude < 0.01f)
        {
            aimIndicator.pointHorizontalAngle = 0f;
            aimIndicator.pointVerticalAngle = 0f;
            return;
        }

        float pointHorizontalAngle = AngleUtil.AngleOffAroundAxis(_rb.velocity, transform.forward, Vector3.up);

        aimIndicator.pointHorizontalAngle = pointHorizontalAngle;

        // pointVerticalAngle

        float pointVerticalAngle = AngleUtil.AngleOffAroundAxis(_rb.velocity, transform.forward, noseLeft);

        aimIndicator.pointVerticalAngle = pointVerticalAngle;


        aimIndicator.altude = _rb.position.y;

        aimIndicator.speed = frontSpeed;

        aimIndicator.verticalSpeed = verticalSpeed;

        aimIndicator.acceleration = frontAcceleration;


    }
}
