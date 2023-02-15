using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    HingeJoint joint = null;

    public bool state = false;

    void Start()
    {
        joint = GetComponent<HingeJoint>();

        state = false;
    }

    public void UseBreak()
    {
        joint.useSpring = true;

        state = true;
    }

    public void ReleaseBreak()
    {
        joint.useSpring = false;

        state = false;
    }
}
