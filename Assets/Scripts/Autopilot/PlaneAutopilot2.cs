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

            needPitch = GetNeedPitch();

            var rudder = GetRudder();

            needRudders = positive ? rudder * -1 : rudder;

            //needRudders = positive ? rudder * -1 : rudder;
        }

        airPlane.SetAilerons(-(needRoll + airPlane.roll) / 40f);

        airPlane.SetElevators(-(needPitch + airPlane.pitch) / 10f);

        airPlane.SetRudders(-needRudders);

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
            return 8f;
        else
        {
            var targetAngle = -navigator.GetVerticalAngle(navigator.CurPoint);

            //var result = VelocityVerticalAngle - targetAngle;

            var v = ((targetAngle* Mathf.Sign(VelocityVerticalAngle) - VelocityVerticalAngle) / 100f) ;

            return needPitch - v;

            if (VelocityVerticalAngle < targetAngle)
                return needPitch + v;
            else
                return needPitch - v;

            //var needElevationSpeed = (navigator.CurPoint.position.y - transform.position.y) / navigator.GetTimeToNextPoint(airPlane.frontSpeed);

            //var realElevationSpeed = airPlane.elevationSpeed;

            //var ToUp = Mathf.Sign(needElevationSpeed) > 0f;
            //var ElevationUp = Mathf.Sign(realElevationSpeed) > 0f;

            //if (ToUp)
            //{
            //    if (needElevationSpeed > realElevationSpeed)
            //        return needPitch + 0.001f;
            //    else
            //        return needPitch - 0.001f;
            //}
            //else
            //{
            //    if (needElevationSpeed < realElevationSpeed)
            //        return needPitch - 0.001f;
            //    else
            //        return needPitch + 0.001f;
            //}
        }
    }

    private float GetThrottle()
    {
        float speedError = needSpeed - airPlane.frontSpeed;

        return _enginePID.GetOutput(speedError, Time.fixedDeltaTime);
    }

    private float GetRudder()
    {
        if (CurMode == AutopilotMode.Turn)
            return 8f;

        return 0f;
    }

    public enum AutopilotMode
    {
        Turn,
        Correction,
        Hold
    }
}
