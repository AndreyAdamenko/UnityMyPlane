using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAutopilot2 : MonoBehaviour
{
    float selectedAltitude = 0f;

    Rigidbody _rb = null;

    AirPlane airPlane = null;

    public float needRoll = 0f;

    public float needPitch = 0f;

    public float needRudders = 0f;

    public float needSpeed = 65f;

    private Navigator navigator;

    //[SerializeField]
    //[Range(-1, 1)]
    //private float _P, _I, _D;

    [SerializeField]
    public PID.PidParameters engeenPidParameters;

    [SerializeField]
    public PID.PidParameters elevatorPidParameters;

    private PID _enginePID;
    private PID _elevatorPID;

    float curThrottle = 0f;

    public string CurModeTitle = "Hold";
    private AutopilotMode _curMode = AutopilotMode.Hold;
    public AutopilotMode CurMode
    {
        get => _curMode;

        private set
        {
            _curMode = value;
            CurModeTitle = value.ToString();
        }
    }

    public float VelocityVerticalAngle;

    private void Update()
    {
        VelocityVerticalAngle = airPlane.GetVelocityVerticalAngle();
    }

    private void Awake()
    {
        _enginePID = new PID(engeenPidParameters, 1f);

        _elevatorPID = new PID(elevatorPidParameters, 0.1f);

        _rb = GetComponent<Rigidbody>();

        airPlane = GetComponent<AirPlane>();

        navigator = GetComponent<Navigator>();
    }

    private void FixedUpdate()
    {
        if (navigator != null)
        {
            var horizontalAngleOnPoint = navigator.GetHorizontalAngle(navigator.CurPoint);

            var positive = horizontalAngleOnPoint > 0;

            var value = Mathf.Abs(horizontalAngleOnPoint);

            if (value > 15f)
            {
                CurMode = AutopilotMode.Turn;
            }
            else if ((CurMode == AutopilotMode.Turn && value < 2f) || (CurMode == AutopilotMode.Hold && value > 2f))
            {
                CurMode = AutopilotMode.Correction;
            }
            else if (value < 0.1f)
            {
                CurMode = AutopilotMode.Hold;
            }

            float roll = GetRoll();

            needRoll = positive ? roll * -1 : roll;

            needPitch = GetNeedPitch2();


            //needRudders = positive ? rudder * -1 : rudder;
        }

        airPlane.SetAilerons((-needRoll - airPlane.roll) / 40f);

        airPlane.SetElevators((-needPitch - airPlane.pitch) / 10f);

        //airPlane.SetRudders(-needRudders);

        curThrottle = GetThrottle();

        airPlane.SetThrottle(curThrottle);
    }

    private float GetRoll()
    {
        switch (_curMode)
        {
            case AutopilotMode.Turn:
                return 45f;

            case AutopilotMode.Correction:
                return 1f;

            case AutopilotMode.Hold:
                return 0f;

            default:
                return 0f;
        }
    }

    private float GetNeedPitch()
    {
        if (CurMode == AutopilotMode.Turn)
            return 8;
        else
        {
            float elevatorError = navigator.CurPoint.position.y - transform.position.y;

            var pidValue = _elevatorPID.GetOutput(elevatorError, Time.fixedDeltaTime);

            return Mathf.Abs(pidValue) > 35 ? 35 * Mathf.Sign(pidValue) : pidValue;
        }
    }

    private float GetNeedPitch2()
    {
        if (CurMode == AutopilotMode.Turn)
            return 8;
        else
        {
            var planeVelocityVerticalAngle = airPlane.GetVelocityVerticalAngle();
            var pointVerticalAngle = navigator.GetVerticalAngle(navigator.CurPoint);
            
            float angleError = (planeVelocityVerticalAngle - pointVerticalAngle);

            var pidValue = _elevatorPID.GetOutput(-angleError, Time.fixedDeltaTime);

            return Mathf.Abs(pidValue) > 35 ? 35 * Mathf.Sign(pidValue) : pidValue;

            //return navigator.GetVerticalAngle(navigator.CurPoint);
        }
    }

    private float GetNeedPitch3()
    {
        if (CurMode == AutopilotMode.Turn)
            return 8;
        else
        {
            float elevatorError = navigator.CurPoint.position.y - transform.position.y;

            var pidValue = _elevatorPID.GetOutput(elevatorError, Time.fixedDeltaTime);

            return Mathf.Abs(pidValue) > 35 ? 35 * Mathf.Sign(pidValue) : pidValue;
        }
    }

    private float GetThrottle()
    {
        float speedError = needSpeed - airPlane.frontSpeed;

        return _enginePID.GetOutput(speedError, Time.fixedDeltaTime);
    }

    public enum AutopilotMode
    {
        Turn,
        Correction,
        Hold
    }
}
