using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CPInput
{
    public static float GetAxis(string name)
    {
        float result = 0f;

        result = Input.GetAxis(name);

        return result;
    }

}
