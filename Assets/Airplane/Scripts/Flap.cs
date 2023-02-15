using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flap : MonoBehaviour
{
    [SerializeField]
    float maxAngle = 10;

    [SerializeField]
    float speed = 0.01f;

    Quaternion startRotation = Quaternion.identity;

    float selectedValue = 0f;

    InertedProcess flapPosition = new InertedProcess(0f, 1f);

    //float curValue = 0f;

    public float SelectedValue { 
        get => selectedValue;
        set
        {
            if (value > 1f)
            {
                selectedValue = 1f;
            }
            else if (value < -1f)
            {
                selectedValue = -1;
            }
            else
            {
                selectedValue = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        flapPosition.CalculateAccumulatedValue(selectedValue, speed);


        transform.localRotation = Quaternion.Euler((maxAngle * -flapPosition.curValue) + startRotation.eulerAngles.x, 0, 0);
    }
}
