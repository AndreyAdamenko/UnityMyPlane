using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField]
    List<Transform> routePoints = new List<Transform>();
    public Transform CurPoint { get; private set; }

    Rigidbody _rb = null;

    private void Awake()
    {
        if (routePoints.Count > 0)
            CurPoint = routePoints[0];

        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (CurPoint != null)
        {
            var pointPosition = CurPoint.transform.position;
            var myPosition = transform.position;

            var flatPointPosition = new Vector2(pointPosition.x, pointPosition.z);
            var flatMyPosition = new Vector2(myPosition.x, myPosition.z);
            
            if ((flatPointPosition - flatMyPosition).magnitude < 50)
            {
                var curIndex = routePoints.IndexOf(CurPoint);

                if (routePoints.Count > curIndex + 1)
                {
                    CurPoint = routePoints[curIndex + 1];
                }
                else
                    CurPoint = routePoints[0];
            }
        }
    }

    public float GetDistance()
    {
        if (CurPoint != null)
            return (CurPoint.transform.position - _rb.transform.position).magnitude;
        else
            return float.MaxValue;
    }

    public float GetHorizontalAngle()
    {
        if (CurPoint == null) return 0;

        var vectorToTarget = CurPoint.transform.position - transform.position;

        return AngleUtil.AngleOffAroundAxis(vectorToTarget, _rb.velocity.normalized, Vector3.up);
    }
}
