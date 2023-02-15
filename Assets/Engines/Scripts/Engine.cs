using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : RigidbodyFinder
{
    [SerializeField]
    private GameObject flame = null;

    [SerializeField]
    private float forceMax = 10;

    [SerializeField]
    private FuelTank tank;

    [SerializeField]
    float speed = 0.01f;


    /// <summary>
    /// Consumption per unit of power
    /// </summary>
    [SerializeField]
    private float fuelConsumption = 0.001f;

    private float selectedTraction = 0;

    private bool thereIsFuel;

    InertedProcess engineTrotle = new InertedProcess(0f, 1f);

    public float curTruction = 0f;

    public float curForce = 0f;

    private AudioSource audioS = null;

    public float SelectedTraction
    {
        get => selectedTraction;
        set
        {
            selectedTraction = value;
            if (value > 1f) selectedTraction = 1f;
            if (value < 0f) selectedTraction = 0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();

        if (tank == null) Debug.LogWarning(transform.gameObject.name + ": Tank not set!");

        getRigidbody();
    }

    private void Update()
    {
        SetFlame();

        if (!thereIsFuel)
        {
            audioS.StopSound();

            return;
        }

        audioS.SetSound(engineTrotle.curValue);


    }

    private void FixedUpdate()
    {
        GetPower();
    }

    private void GetPower()
    {
        if (tank == null) return;

        engineTrotle.CalculateAccumulatedValue(selectedTraction, speed);

        curTruction = engineTrotle.curValue;

        curForce = forceMax * engineTrotle.curValue;

        Vector3 relativeForce = Vector3.forward * curForce;

        thereIsFuel = tank.GetFuel(GetCurConsumption(curForce) * Time.fixedDeltaTime);

        if (thereIsFuel)
        {
            _rb.AddForceAtPosition(transform.TransformDirection(relativeForce), transform.position);
        }
    }

    private void SetFlame()
    {
        flame.transform.localScale = new Vector3(1, 1, engineTrotle.curValue);

        if (thereIsFuel)
        {
            if (engineTrotle.curValue >= 0.5f) flame.SetActive(true);
            if (engineTrotle.curValue < 0.5f) flame.SetActive(false);
        }
        else
        {
            flame.SetActive(false);
        }
    }

    private float GetCurConsumption(float force)
    {
        float efficiencyRatio = 0;

        float forcePercent = force / forceMax;

        if (forcePercent < 0.5f)
        {
            efficiencyRatio = 1.2f;
        }
        else if (force >= 0.5f)
        {
            efficiencyRatio = 1f;
        }
        else if (force >= 0.8f)
        {
            efficiencyRatio = 1.6f;
        }

        return efficiencyRatio * force * fuelConsumption;
    }
}
