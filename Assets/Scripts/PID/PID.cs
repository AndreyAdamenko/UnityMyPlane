using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class PID
{
    private float _p, _i, _d;
    private float _prevError;

    private float _multiplier;

    public PidParameters pidParameters;

    public PID(PidParameters pidParameters, float multiplier)
    {
        _multiplier = multiplier;
        this.pidParameters = pidParameters;
        _p = pidParameters.P * multiplier;
        _i = pidParameters.I * multiplier;
        _d = pidParameters.D * multiplier;
    }

    /// <summary>
    /// Based on the code from Brian-Stone on the Unity forums
    /// https://forum.unity.com/threads/rigidbody-lookat-torque.146625/#post-1005645
    /// </summary>
    /// <param name="currentError"></param>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    public float GetOutput(float currentError, float deltaTime)
    {
        _p = currentError;
        _i += _p * deltaTime;
        _d = (_p - _prevError) / deltaTime;
        _prevError = currentError;

        return _p * (pidParameters.P * _multiplier) + 
               _i * (pidParameters.I * _multiplier) + 
               _d * (pidParameters.D * _multiplier);
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