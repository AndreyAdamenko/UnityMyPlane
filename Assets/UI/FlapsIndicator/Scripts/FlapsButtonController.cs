using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlapsButtonController : MonoBehaviour
{
    public FlapsController flapsController = null;
    
    public RectTransform flapImage = null;

    [SerializeField]
    Image buttonImage = null;

    [SerializeField]
    public Text text = null;

    private void Awake()
    {
        //buttonImage = GetComponent<Image>();

        //text = transform.GetComponentInChildren<Text>();
    }

    public void ChangeTo(FlapsPosition position)
    {
        buttonImage.color = position.indicatorColor;

        text.text = "Flaps: " + position.name;

        flapImage.localRotation = Quaternion.Euler(0, 0, -position.indicatorAngle);
    }

    public void ChangePosition(int direction)
    {
        flapsController.ChangePosition(direction);
    }
}
