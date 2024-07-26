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

    public float autoPitchMax = 10f;
    public float autoPitchMin = -10f;

    private Navigator navigator;

    private FlapsController flaps;

    private bool breakApplied = false;


    float lastAltitude = 0;
    public float verticalSpeed = 0;

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

    public string CurTurnTitle = "Hold";
    private AutopilotMode _curMode = AutopilotMode.Hold;
    public AutopilotMode TurnCurMode
    {
        get => _curMode;

        private set
        {
            _curMode = value;
            CurTurnTitle = value.ToString();
        }
    }

    public string CurSpeedTitle = "Hold";
    private AutoSpeedMode _curSpeedMode = AutoSpeedMode.Hold;
    public AutoSpeedMode SpeedCurMode
    {
        get => _curSpeedMode;

        private set
        {
            _curSpeedMode = value;
            CurSpeedTitle = value.ToString();
        }
    }

    public string CurElevModeTitle = "Hold";
    private AutoElevMode _curElevMode = AutoElevMode.Hold;
    public AutoElevMode ElevCurMode
    {
        get => _curElevMode;

        private set
        {
            _curElevMode = value;
            CurElevModeTitle = value.ToString();
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

        //_elevatorPID = new PID(elevatorPidParameters, 0.1f);

        _rb = GetComponent<Rigidbody>();

        airPlane = GetComponent<AirPlane>();

        navigator = GetComponent<Navigator>();

        flaps = GetComponent<FlapsController>();
    }

    private void FixedUpdate()
    {
        verticalSpeed = (transform.position.y - lastAltitude) * (1 / Time.fixedDeltaTime);

        lastAltitude = transform.position.y;

        if (navigator != null)
        {
            var horizontalAngleOnPoint = navigator.GetHorizontalAngle(navigator.CurPoint);

            var positive = horizontalAngleOnPoint > 0;

            CheckTurnMode(horizontalAngleOnPoint);

            float roll = GetRoll();

            needRoll = positive ? roll * -1 : roll;

            // Elevation
            CheckElevation();
            SetPitch();

            var rudder = GetRudder();

            needRudders = positive ? rudder * -1 : rudder;

            //needRudders = positive ? rudder * -1 : rudder;
        }



        airPlane.SetAilerons(-(needRoll + airPlane.roll) / 40f);

        airPlane.SetElevators(-(needPitch + airPlane.pitch) / 10f);

        airPlane.SetRudders(-needRudders);

        // Speed
        CheckSpeed();
        SetBreaks();
        SetThrottle();
        airPlane.SetThrottle(curThrottle);

        void CheckSpeed()
        {
            float speedError = needSpeed - airPlane.frontSpeed;

            if (speedError > 20f)
            {
                SpeedCurMode = AutoSpeedMode.Up;
            }
            else if (speedError < -20f)
            {
                SpeedCurMode = AutoSpeedMode.Down;
            }

            if (SpeedCurMode != AutoSpeedMode.Hold && Mathf.Abs(speedError) < 5f)
            {
                SpeedCurMode = AutoSpeedMode.Hold;
            }
        }

        void CheckTurnMode(float horizontalAngleOnPoint)
        {
            var value = Mathf.Abs(horizontalAngleOnPoint);

            if (value > 15f)
            {
                TurnCurMode = AutopilotMode.Turn;
            }
            else if ((TurnCurMode == AutopilotMode.Turn && value < 2f) || (TurnCurMode == AutopilotMode.Hold && value > 2f))
            {
                TurnCurMode = AutopilotMode.Correction;
            }
            else if (value < 0.1f)
            {
                TurnCurMode = AutopilotMode.Hold;
            }
        }

        void SetBreaks()
        {
            switch (SpeedCurMode)
            {
                case AutoSpeedMode.Up:
                    if (breakApplied)
                    {
                        flaps.Up();
                        breakApplied = false;
                    }
                    break;

                case AutoSpeedMode.Hold:
                    if (breakApplied)
                    {
                        flaps.Up();
                        breakApplied = false;
                    }
                    break;

                case AutoSpeedMode.Down:
                    flaps.Full();
                    breakApplied = true;
                    break;
            }
        }

        void SetThrottle()
        {
            switch (SpeedCurMode)
            {
                case AutoSpeedMode.Up:

                    curThrottle = 100f;
                    RemovePid();

                    break;

                case AutoSpeedMode.Hold:

                    AddPid();

                    float speedError = needSpeed - airPlane.frontSpeed;
                    curThrottle = _enginePID.GetOutput(speedError, Time.fixedDeltaTime);

                    break;

                case AutoSpeedMode.Down:

                    curThrottle = 0f;
                    RemovePid();

                    break;
            }

            void RemovePid()
            {
                if (_enginePID != null) _enginePID = null;
            }

            void AddPid()
            {
                if (_enginePID == null) _enginePID = new PID(engeenPidParameters, 1f);
            }
        }

        void CheckElevation()
        {
            float elevatorError = navigator.CurPoint.position.y - transform.position.y;

            if (elevatorError > 20f) // Цель выше
            {
                ElevCurMode = AutoElevMode.Up;
            }
            else if (elevatorError < -20f) // Цель ниже
            {
                ElevCurMode = AutoElevMode.Down;
            }

            if (ElevCurMode != AutoElevMode.Hold && Mathf.Abs(elevatorError) < 10f)
            {
                ElevCurMode = AutoElevMode.Hold;
            }
        }

        void SetPitch()
        {
            switch (ElevCurMode)
            {
                case AutoElevMode.Up:
                    needPitch = autoPitchMax;
                    RemovePid();
                    break;

                case AutoElevMode.Hold:

                    AddPid();

                    float elevatorError = navigator.CurPoint.position.y - transform.position.y;
                    var pidValue = _elevatorPID.GetOutput(elevatorError, Time.fixedDeltaTime);
                    needPitch = Mathf.Abs(pidValue) > 35 ? 35 * Mathf.Sign(pidValue) : pidValue;

                    break;

                case AutoElevMode.Down:
                    needPitch = autoPitchMin;
                    RemovePid();
                    break;
            }

            void RemovePid()
            {
                if (_elevatorPID != null) _elevatorPID = null;
            }

            void AddPid()
            {
                if (_elevatorPID == null) _elevatorPID = new PID(elevatorPidParameters, 0.1f);
            }
        }
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

    private float GetNeedPitch2()
    {
        float elevatorError = navigator.CurPoint.position.y - transform.position.y;

        if (elevatorError > 10)
        {
            _elevatorPID = null;
            return 5;
        }
        else if (elevatorError < -10)
        {
            _elevatorPID = null;
            return -2;
        }


        if (_elevatorPID is null)
            _elevatorPID = new PID(elevatorPidParameters, 0.1f);

        var pidValue = _elevatorPID.GetOutput(elevatorError, Time.fixedDeltaTime);

        return Mathf.Abs(pidValue) > 35 ? 35 * Mathf.Sign(pidValue) : pidValue;

        //if (verticalSpeed < 2)
        //    return 5;
        //else if (verticalSpeed > 2)
        //    return -5;
        //else
        //    return 2;
    }

    private float GetNeedPitch()
    {
        if (TurnCurMode == AutopilotMode.Turn)
            return 8f;
        else
        {
            var targetAngle = -navigator.GetVerticalAngle(navigator.CurPoint);

            //var result = VelocityVerticalAngle - targetAngle;

            var v = ((targetAngle * Mathf.Sign(VelocityVerticalAngle) - VelocityVerticalAngle) / 100f);

            //return needPitch - v;

            if (VelocityVerticalAngle < targetAngle)
                return needPitch - v;
            else
                return needPitch + v;

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
        if (TurnCurMode == AutopilotMode.Turn)
            return 8f;

        return 0f;
    }

    public enum AutopilotMode
    {
        Turn,
        Correction,
        Hold
    }

    public enum AutoSpeedMode
    {
        Up,
        Hold,
        Down
    }

    public enum AutoElevMode
    {
        Up,
        Hold,
        Down
    }
}
