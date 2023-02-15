using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompassSensor : MonoBehaviour
{
    [SerializeField]
    List<Transform> targets = new List<Transform>();

    Dictionary<Transform, CompasIndicatorProvider> compasArrows = new Dictionary<Transform, CompasIndicatorProvider>();

    public float compassValue = 0;

    AirPlane airPlane = null;

    private void Awake()
    {
        airPlane = GetComponent<AirPlane>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.TransformPoint(0, 0, 0) + new Vector3(0, 2, 0), transform.TransformPoint(0, 0, 0) + new Vector3(0, 2, 2), Color.magenta);

        Debug.DrawLine(transform.TransformPoint(0, 0, 0) + new Vector3(0, 2, 0), transform.TransformPoint(0, 0, 0) + new Vector3(0, 2, 0) + new Vector3(transform.forward.x, 0, transform.forward.z).normalized, Color.magenta);
    }

    public void AddTarget(Transform transform)
    {
        if (targets.FindAll(t => t == transform).Count != 0) return;

        targets.Add(transform);

        compasArrows.Add(transform, Compass.instance.AddTarget());
    }

    public void RemoveTarget(Transform transform)
    {
        bool removeResult = targets.Remove(transform);

        if (removeResult)
        {
            CompasIndicatorProvider indicator = compasArrows[transform];

            Compass.instance.RemoveTarget(indicator);

            compasArrows.Remove(transform);
        }
    }

    void FixedUpdate()
    {
        //if (airPlane.isActive) 
            Calculate();
    }

    private void Calculate()
    {
        compassValue = AngleUtil.AngleOffAroundAxis(Vector3.forward, transform.forward, Vector3.up);

        foreach (Transform curTarget in targets)
        {
            if (curTarget == null) continue;
            
            Vector3 targetVector = curTarget.position - transform.position;

            CompasIndicatorProvider indicator = null;

            if (!compasArrows.ContainsKey(curTarget))
            {
                indicator = Compass.instance.AddTarget();

                compasArrows.Add(curTarget, indicator);
            }
            else
            {
                indicator = compasArrows[curTarget];
            }

            indicator.Angle = AngleUtil.AngleOffAroundAxis(targetVector, transform.forward, Vector3.up);
        }
    }

    public void DiscardCompassArrows()
    {
        compasArrows.Clear();
    }
}
