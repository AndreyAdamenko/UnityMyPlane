using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMass : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField]
    Vector3 centerMass;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        centerMass = _rb.centerOfMass;

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        sphere.transform.parent = transform;

        sphere.name = "CenterOfMass";

        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        sphere.transform.position = transform.TransformPoint(centerMass);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
