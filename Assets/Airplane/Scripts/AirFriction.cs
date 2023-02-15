using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirFriction : RigidbodyFinder
{
    Air air = null;

    public Vector3 dimensions = new Vector3(5f, 0.001f, 2f);

    [Tooltip("Lift coefficient curve.")]
    public WingCurves wing;

    AudioSource airAudio = null;

    Vector3 oldPos = Vector3.zero;

    [SerializeField]
    Vector3 relativeForce = Vector3.zero;

    Vector3 relativeVelocity = Vector3.zero;

    [SerializeField]
    bool liftForce = true;

    [SerializeField]
    float yVelocityAngle = 0f;

    public float WingAreaXY
    {
        get { return dimensions.x * dimensions.y; }
    }

    public float WingAreaXZ
    {
        get { return dimensions.x * dimensions.z; }
    }

    public float WingAreaYZ
    {
        get { return dimensions.y * dimensions.z; }
    }

    // Start is called before the first frame update
    void Start()
    {
        oldPos = transform.position;

        getRigidbody();

        air = GameObject.Find("Air").GetComponent<Air>();

        airAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        Vector3 worldVelocity = transform.TransformPoint(relativeVelocity);

        Debug.DrawLine(transform.position, worldVelocity, Color.red);

        Vector3 worldForce = transform.TransformPoint(relativeForce);

        Debug.DrawLine(transform.position, worldForce, Color.green);

#endif
    }

    private void FixedUpdate()
    {
        if (air == null) return;

        CalculateVelocity();

        GetForce();

        if (airAudio == null) return;

        float velocity = _rb.velocity.magnitude;

        SetSound(velocity, air.density);
    }

    private void CalculateVelocity()
    {
        Vector3 oldRelativePosition = transform.InverseTransformPoint(oldPos);
        
        relativeVelocity = -oldRelativePosition;

        oldPos = transform.position;

    }

    private void GetForce()
    {
        float xForce = (air.density * (relativeVelocity.x * relativeVelocity.x * Mathf.Sign(relativeVelocity.x)) / 2) * WingAreaYZ;

        float yForce = (air.density * (relativeVelocity.y * relativeVelocity.y * Mathf.Sign(relativeVelocity.y)) / 2) * WingAreaXZ;

        float zForce = (air.density * (relativeVelocity.z * relativeVelocity.z * Mathf.Sign(relativeVelocity.z)) / 2) * WingAreaXY;

        relativeForce = -(new Vector3(xForce, yForce, zForce));

        _rb.AddForceAtPosition(transform.TransformDirection(relativeForce), transform.position);
    }

    private void SetSound(float velocity, float airDensity)
    {
        //Speed calculate
        float highSpeed = 300;

        float lowSpeed = 10;

        float audioLowSpeedFactor = (((velocity) / (lowSpeed)) * 0.2f);

        float audioHighSpeedFactor = (((velocity) / (highSpeed)) * 0.8f);

        float volume = ((audioHighSpeedFactor + audioLowSpeedFactor) * 0.6f) * airDensity;

        float pitch = (audioHighSpeedFactor + audioLowSpeedFactor) * 0.4f;

        if (volume <= 0.05f)
        {
            if (airAudio.isPlaying) airAudio.Stop();
        }

        if (pitch > 0.05f)
        {
            if (!airAudio.isPlaying) airAudio.Play();
        }

        if (airAudio.isPlaying)
        {
            airAudio.volume = volume;

            airAudio.pitch = pitch;
        }
    }

    // Prevent this code from throwing errors in a built game.
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Matrix4x4 oldMatrix = Gizmos.matrix;

        Gizmos.color = Color.cyan;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(dimensions.x, dimensions.y, dimensions.z));

        Gizmos.matrix = oldMatrix;
    }

    public void GetScale()
    {
        Vector3 scale = transform.localScale;

        dimensions = scale;
    }

    private void DrawLocalLine(Vector3 to, Color color)
    {
        DrawLine(transform.position, transform.rotation * to, Vector3.zero, color);
    }

    private void DrawLine(Vector3 from, Vector3 to, Vector3 offset, Color color)
    {
        Debug.DrawLine(from + offset, to + from + offset, color);
    }
#endif
}
