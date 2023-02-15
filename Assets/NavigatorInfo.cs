using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigatorInfo : MonoBehaviour
{
    [SerializeField]
    private Navigator _navigator;

    [SerializeField]
    private Text horizontalAngleText;

    [SerializeField]
    private Text verticalAngleText;

    [SerializeField]
    private Text distanceText;

    [SerializeField]
    private Text titleText;

    private string _title;
    private string Title
    {
        get => _title;

        set
        {
            _title = value;
            titleText.text = $"Point: {_title}";
        }
    }

    private float _horizontalAngle;
    private float HorizontalAngle
    {
        get => _horizontalAngle;

        set
        {
            _horizontalAngle = value;
            horizontalAngleText.text = $"HA: {_horizontalAngle.ToString("0.00")}";
        }
    }

    private float _verticalAngle;
    private float VerticalAngle
    {
        get => _verticalAngle;

        set
        {
            _verticalAngle = value;
            verticalAngleText.text = $"VA: {_verticalAngle.ToString("0.00")}";
        }
    }

    private float _distance;
    private float Distance
    {
        get => _distance;

        set
        {
            _distance = value;

            if (_distance <= 1000000)
                distanceText.text = $"Dist: {_distance.ToString("0.00")}";
            else
                distanceText.text = $"Dist: 1M+";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_navigator == null) return;

        Distance = _navigator.GetDistance();
        HorizontalAngle = _navigator.GetHorizontalAngle(_navigator.CurPoint);
        VerticalAngle = _navigator.GetVerticalAngle(_navigator.CurPoint);
        Title = _navigator.CurPoint == null ? "---" : _navigator.CurPoint.gameObject.name;
    }
}
