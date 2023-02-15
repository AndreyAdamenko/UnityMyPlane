using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : RigidbodyFinder
{
    //Rigidbody _rb;

    [SerializeField]
    float forceValue = 10;

    Vector3 curForceVector = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        getRigidbody();

        //_rb.maxAngularVelocity = 500;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldForce = transform.TransformPoint(curForceVector);

        Debug.DrawLine(transform.position, worldForce, Color.yellow);
    }

    private void FixedUpdate()
    {
        //float horiz = Input.GetAxis("Horizontal2");

        float horiz = 1;

        _rb.AddRelativeTorque(Vector3.forward * horiz * forceValue, ForceMode.Force);

        curForceVector = Vector3.forward * (transform.InverseTransformDirection(_rb.angularVelocity).z * 0.5f);

        _rb.AddRelativeForce(curForceVector, ForceMode.Force);
    }
}
