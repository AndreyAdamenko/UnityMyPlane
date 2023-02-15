using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleScaleBarController : MonoBehaviour
{
    public static AngleScaleBarController instance = null;
    
    [SerializeField]
    GameObject arrowPrefab = null;

    List<AngleProvider> angles = new List<AngleProvider>();

    public float maxAngle = 90f;

    public float maxPosition = 300f;

    public float arrowsOffset = 56f;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        foreach (AngleProvider angle in angles)
        {
            float position = (angle.Angle / maxAngle) * maxPosition;

            float offset = angle.side == ArrowSide.right ? arrowsOffset : -arrowsOffset;

            angle.arrowRect.localPosition = new Vector3(offset, position);
        }
    }

    public AngleProvider AddAngle(ArrowSide side)
    {
        float offset = side == ArrowSide.right ? arrowsOffset : -arrowsOffset;

        GameObject newArrow = Instantiate(arrowPrefab, new Vector3(offset, 0), arrowPrefab.transform.rotation, transform);

        AngleProvider newAngle = new AngleProvider(side, newArrow);

        angles.Add(newAngle);

        return newAngle;
    }
}

[System.Serializable]
public class AngleProvider
{
    private float angle = 0f;

    public ArrowSide side = ArrowSide.Left;

    public RectTransform arrowRect = null;

    private Text text = null;

    public float Angle { 
        get => angle;
        set
        {
            if (text == null)
            {
                text = arrowRect.gameObject.GetComponentInChildren<Text>();
            }

            text.text = value.ToString("0");

            angle = value;
        }
    }

    public AngleProvider(ArrowSide side, GameObject arrow)
    {
        this.side = side;
        this.arrowRect = arrow.GetComponent<RectTransform>();
    }
}

public enum ArrowSide
{
    Left = 0,
    right = 1
}
