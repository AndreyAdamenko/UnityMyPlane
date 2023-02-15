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
    private string title
    {
        get => _title;

        set
        {
            _title = value;
            titleText.text = $"Point: {_title}";
        }
    }

    private float _horizontalAngle;
    private float horizontalAngle
    {
        get => _horizontalAngle;

        set
        {
            _horizontalAngle = value;
            horizontalAngleText.text = $"HA: {_horizontalAngle.ToString("0.00")}";
        }
    }

    private float _verticalAngle;
    private float verticalAngle
    {
        get => _verticalAngle;

        set
        {
            _verticalAngle = value;
            verticalAngleText.text = $"HA: {_verticalAngle.ToString("0.00")}";
        }
    }

    private float _distance;
    private float distance
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

        distance = _navigator.GetDistance();
        horizontalAngle = _navigator.GetHorizontalAngle();
        title = _navigator.CurPoint == null ? "---" : _navigator.CurPoint.gameObject.name;
    }
}
