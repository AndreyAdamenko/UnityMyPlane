using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyFinder : MonoBehaviour
{
    protected Rigidbody _rb;

    protected int maxRigidbodyNestingLevel = 3;

    protected void getRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            _rb = rb;
            return;
        }

        Transform curParentTransform = transform.parent;

        for (int i = 0; i < maxRigidbodyNestingLevel; i++)
        {
            if (curParentTransform == null) break;

            GameObject curParent = curParentTransform.gameObject;

            if (curParent == null) break;

            rb = curParent.GetComponent<Rigidbody>();

            if (rb != null)
            {
                _rb = rb;
                return;
            }

            curParentTransform = curParentTransform.parent;
        }
    }
}
