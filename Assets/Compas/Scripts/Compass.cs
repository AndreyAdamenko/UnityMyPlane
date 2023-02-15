using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public static Compass instance = null;

    public CompassSensor sensor = null;

    [SerializeField]
    GameObject targetArrowPrefab = null;

    [SerializeField]
    float targetArrowOffset = 0f;

    List<CompasIndicatorProvider> targetArrows = new List<CompasIndicatorProvider>();

    RectTransform farme = null;

    #region symbolLine

    GameObject symbolLine = null;

    RectTransform mainImageRect = null;

    RectTransform secondImageRect = null;

    float imageWidth = 0;

    float increment = 0;

    #endregion

    //RectTransform ArrowTarget = null;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        #region symbolLine

        symbolLine = gameObject.transform.Find("Panel/Mask/Symbols").gameObject;

        mainImageRect = symbolLine.transform.GetComponentInChildren<RectTransform>();

        secondImageRect = Instantiate(mainImageRect.gameObject, mainImageRect.transform.parent).GetComponent<RectTransform>();

        imageWidth = mainImageRect.rect.width;

        increment = imageWidth / 360;

        #endregion

        farme = GetComponent<RectTransform>();

        //ArrowTarget = transform.Find("Panel/Mask/ArrowTarget").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawSymbolLine();

        DrawTargetArrow();
    }


    private void DrawSymbolLine()
    {
        symbolLine.transform.position = transform.TransformPoint(new Vector3(sensor.compassValue * increment, 0, 0));

        secondImageRect.localPosition = new Vector2(mainImageRect.localPosition.x + (imageWidth * (sensor.compassValue >= 0 ? -1 : 1)), secondImageRect.localPosition.y);
    }

    private void DrawTargetArrow()
    {
        foreach (CompasIndicatorProvider target in targetArrows)
        {
            float position = target.Angle * increment;

            if (position > farme.rect.width / 2) position = farme.rect.width / 2;

            if (position < -farme.rect.width / 2) position = -farme.rect.width / 2;

            target.arrowRect.localPosition = new Vector3(position, targetArrowOffset);
        }
    }

    public CompasIndicatorProvider AddTarget()
    {
        Transform maskTransform = transform.Find("Panel/Mask");

        GameObject newArrow = Instantiate(targetArrowPrefab, maskTransform);

        CompasIndicatorProvider newTarget = new CompasIndicatorProvider(newArrow);

        targetArrows.Add(newTarget);

        return newTarget;
    }

    public void RemoveTarget(CompasIndicatorProvider indicator)
    {
        Destroy(indicator.arrowRect.gameObject);

        targetArrows.Remove(indicator);
    }

    public void CleanArrows()
    {
        foreach (CompasIndicatorProvider target in targetArrows)
        {
            Destroy(target.arrowRect.gameObject);
        }

        targetArrows.Clear();
    }
}

[System.Serializable]
public class CompasIndicatorProvider
{
    private float angle = 0f;

    public RectTransform arrowRect = null;

    //private Text text = null;

    public float Angle
    {
        get => angle;
        set
        {
            //if (text == null)
            //{
            //    text = arrowRect.gameObject.GetComponentInChildren<Text>();
            //}

            //text.text = value.ToString("0");

            angle = value;
        }
    }

    public CompasIndicatorProvider(GameObject arrow)
    {
        this.arrowRect = arrow.GetComponent<RectTransform>();
    }
}
