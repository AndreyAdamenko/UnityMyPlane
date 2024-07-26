using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FlapsPosition
{
    public string name = "name";

    public Color indicatorColor = Color.green;

    /// <summary>
    /// Value from 0 to 1
    /// </summary>
    public float percent = 0f;

    public float indicatorAngle = 0f;
}

public class FlapsController : MonoBehaviour
{
    public List<FlapsPosition> positions = new List<FlapsPosition>();

    public FlapsButtonController button = null;

    public FlapsPosition curPosition = null;

    List<Flap> flaps = new List<Flap>();

    InputAxisController flapsAxis = new InputAxisController(-1f, 1);

    AirPlane airPlane = null;

    private void Start()
    {
        airPlane = GetComponent<AirPlane>();

        flaps.AddRange(transform.GetComponentsInChildren<Flap>());

        curPosition = positions[0];

        FlapsChangeTo(positions[0]);

        button?.ChangeTo(positions[0]);
    }

    private void LateUpdate()
    {
        if (!airPlane.isActive) return;
        
        float value = Input.GetAxis("Flaps");

        if (!flapsAxis.IsValueChanged(value)) return;

        if (value == 1f)
        {
            ChangePosition(1);
        }
        else if (value == -1f)
        {
            ChangePosition(-1);
        }
    }

    public void ChangePosition(int direction)
    {
        if (positions.Count ==0)
        {
            Debug.LogError("Flaps positions list not set");
            return;
        }
        
        int nextPosition = 0;

        int curPositionIndex = positions.FindIndex(e => e == curPosition);

        nextPosition = curPositionIndex + direction;

        if (nextPosition > positions.Count - 1)
        {
            nextPosition = 0;
        }
        else if (nextPosition < 0)
        {
            nextPosition = positions.Count - 1;
        }

        curPosition = positions[nextPosition];

        FlapsChangeTo(curPosition);

        button?.ChangeTo(curPosition);
    }

    public void Full()
    {
        var last = positions.Last();

        FlapsChangeTo(last);

        button?.ChangeTo(last);
    }

    public void Up()
    {
        var first = positions.First();

        FlapsChangeTo(first);

        button?.ChangeTo(first);
    }

    private void FlapsChangeTo(FlapsPosition position)
    {
        foreach (Flap flap in flaps)
        {
            flap.SelectedValue = position.percent;
        }
    }
}
