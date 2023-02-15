using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstForce : MonoBehaviour
{
    [SerializeField]
    int force = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force, ForceMode.Force);
    }
}
