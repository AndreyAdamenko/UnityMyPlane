using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartForce : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField]
    int startForce = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.AddRelativeForce(Vector3.forward * startForce, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
