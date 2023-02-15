using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorHandler
{
    List<Vector3> buffer = new List<Vector3>();

    int bufferSize = 0;

    public VectorHandler(int bufferSize)
    {
        this.bufferSize = bufferSize;
    }

    public Vector3 GetAverageVector(Vector3 newVector)
    {
        buffer.Add(newVector);

        if (buffer.Count > bufferSize) buffer.RemoveAt(0);

        Vector3 result = Vector3.zero;

        int count = 0;

        foreach (Vector3 curForce in buffer)
        {
            result += curForce;

            count++;
        }

        result = result / count;

        return result;
    }
}
