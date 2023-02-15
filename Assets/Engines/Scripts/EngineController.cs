using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineController : MonoBehaviour
{
    public ThrottleLeverUI throttleLever = null;

    [SerializeField]
    float keyboardReaction = 0.01f;

    List<Engine> engines = new List<Engine>();

    float curTraction = 0;

    public AirPlane airPlane = null;

    public float CurTraction
    {
        get => curTraction;
        set
        {
            if (value > 1)
                curTraction = 1;
            else if (value < 0)
                curTraction = 0;
            else
                curTraction = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        engines.AddRange(GetComponentsInChildren<Engine>());

        airPlane = GetComponent<AirPlane>();
    }

    private void FixedUpdate()
    {
        //if (airPlane.isActive)
        //{
        GetNewKeyboardTraction();
        //}

        //GetNewSliderTraction();

        UpdateSlider();

        engines.ForEach(e => e.SelectedTraction = CurTraction);
    }

    private void GetNewKeyboardTraction()
    {
        float addTraction = Input.GetAxis("Throttle") * keyboardReaction;

        if (addTraction == 0) return;

        CurTraction += addTraction;

        throttleLever.Value = CurTraction;
    }

    private void GetNewSliderTraction()
    {
        if (throttleLever == null) return;

        CurTraction = throttleLever.Value;
    }

    private void UpdateSlider()
    {
        if (throttleLever == null) return;

        throttleLever.Value = CurTraction;
    }
}
