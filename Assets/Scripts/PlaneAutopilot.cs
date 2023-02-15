using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAutopilot : MonoBehaviour
{
    float selectedAltitude = 0f;

    Rigidbody _rb = null;

    AirPlane airPlane = null;

    public float needRoll = 0f;

    public float needPitch = 0f;

    public float needRudders = 0f;

    public float needSpeed = 65f;

    private Navigator navigator;

    [SerializeField]
    [Range(-10, 10)]
    private float _P, _I, _D;

    private PID _enginePID;

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
        _enginePID = new PID(_P, _I, _D);
        
        _rb = GetComponent<Rigidbody>();

        airPlane = GetComponent<AirPlane>();

        navigator = GetComponent<Navigator>();
    }

    private void FixedUpdate()
    {
        _enginePID.Kp = _P;
        _enginePID.Ki = _I;
        _enginePID.Kd = _D;


        if (navigator != null)
        {
            var horizontalAngleOnPoint = navigator.GetHorizontalAngle();

            var positive = horizontalAngleOnPoint > 0;

            var value = Mathf.Abs(horizontalAngleOnPoint);

            if (value > 15f)
            {
                CurMode = AutopilotMode.Turn;
            }
            else if ((_curMode == AutopilotMode.Turn && value < 1f) || (_curMode == AutopilotMode.Hold && value > 1f) )
            {
                CurMode = AutopilotMode.Correction;
            }
            else if (value < 0.1f)
            {
                CurMode = AutopilotMode.Hold;
            }

            float roll = GetRoll();

            needRoll = positive ? roll * -1 : roll;

            //needRudders = positive ? rudder * -1 : rudder;
        }

        airPlane.SetAilerons((-needRoll - airPlane.roll) / 40f);

        airPlane.SetElevators((-needPitch - airPlane.pitch) / 10f);

        //airPlane.SetRudders(-needRudders);

        var speedError = needSpeed - airPlane.frontSpeed;

        airPlane.SetThrottle(_enginePID.GetOutput(speedError, Time.fixedDeltaTime) / 100f);
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
}
