using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform target = null;

    Vector3 offset = Vector3.zero;

    //Vector2 camLocalStartPos = Vector3.zero;

    Quaternion camStartRotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        //if (cam != null)
        //{
        offset = new Vector3(0f, 2f, -8f);

        //camLocalStartPos = transform.InverseTransformPoint(transform.position + camStartPos);

        camStartRotation = Quaternion.Euler(10f, 0, 0);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.transform.TransformPoint(offset - new Vector3(0, offset.y, 0)) + new Vector3(0, offset.y, 0);

            transform.rotation = Quaternion.Euler(camStartRotation.eulerAngles.x + target.transform.rotation.eulerAngles.x, camStartRotation.eulerAngles.y + target.transform.rotation.eulerAngles.y, camStartRotation.eulerAngles.z);
        }
    }
}
