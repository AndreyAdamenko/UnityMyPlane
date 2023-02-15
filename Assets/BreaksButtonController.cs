using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreaksButtonController : MonoBehaviour
{
    public BreakController breakController = null;
    
    string message = "Breaks: ";
    
    [SerializeField]
    Color freeColor = Color.green;

    [SerializeField]
    Color pressed = Color.red;

    Text text = null;

    Image buttonImage = null;
    
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponentInChildren<Text>();

        buttonImage = GetComponent<Image>();

        Free();
    }

    public void Press()
    {
        buttonImage.color = pressed;

        text.text = message + "ON";
    }

    public void Free()
    {
        buttonImage.color = freeColor;

        text.text = message + "OFF";
    }

    public void Toggle()
    {
        breakController.Toggle();
    }
}
