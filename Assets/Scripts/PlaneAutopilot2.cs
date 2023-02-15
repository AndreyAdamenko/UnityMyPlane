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
    public PidParameters engeenPidParameters;

    [SerializeField]
    public PidParameters elevatorPidParameters;

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


    private void Awake()
    {
        _enginePID = new PID(
            engeenPidParameters.P,
            engeenPidParameters.I,
            engeenPidParameters.D);

        _elevatorPID = new PID(
            elevatorPidParameters.P,
            elevatorPidParameters.I,
            elevatorPidParameters.D);

        _rb = GetComponent<Rigidbody>();

        airPlane = GetComponent<AirPlane>();

        navigator = GetComponent<Navigator>();
    }

    private void FixedUpdate()
    {
        _enginePID.Kp = engeenPidParameters.P;
        _enginePID.Ki = engeenPidParameters.I;
        _enginePID.Kd = engeenPidParameters.D;

        _elevatorPID.Kp = elevatorPidParameters.P;
        _elevatorPID.Ki = elevatorPidParameters.I;
        _elevatorPID.Kd = elevatorPidParameters.D;


        if (navigator != null)
        {
            var horizontalAngleOnPoint = navigator.GetHorizontalAngle(navigator.CurPoint);

            var positive = horizontalAngleOnPoint > 0;

            var value = Mathf.Abs(horizontalAngleOnPoint);

            if (value > 15f)
            {
                CurMode = AutopilotMode.Turn;
            }
            else if ((_curMode == AutopilotMode.Turn && value < 2f) || (_curMode == AutopilotMode.Hold && value > 2f))
            {
                CurMode = AutopilotMode.Correction;
            }
            else if (value < 0.1f)
            {
                CurMode = AutopilotMode.Hold;
            }

            float roll = GetRoll();

            needRoll = positive ? roll * -1 : roll;


            var elevatorError = navigator.CurPoint.position.y - transform.position.y;

            needPitch = _elevatorPID.GetOutput(elevatorError, Time.fixedDeltaTime);

            //needRudders = positive ? rudder * -1 : rudder;
        }

        airPlane.SetAilerons((-needRoll - airPlane.roll) / 40f);

        airPlane.SetElevators((-needPitch - airPlane.pitch) / 10f);

        //airPlane.SetRudders(-needRudders);

        var speedError = needSpeed - airPlane.frontSpeed;

        curThrottle = _enginePID.GetOutput(speedError, Time.fixedDeltaTime);

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

    public enum AutopilotMode
    {
        Turn,
        Correction,
        Hold
    }

    [Serializable]
    public class PidParameters
    {
        [Range(-1, 5)]
        public float P;

        [Range(-1, 5)]
        public float I;

        [Range(-1, 5)]
        public float D;
    }
}
