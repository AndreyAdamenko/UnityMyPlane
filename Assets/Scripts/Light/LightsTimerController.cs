using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTimerController : MonoBehaviour
{
    [Header("Turn On")]
    public int onHour = 0;
    public int onMinute = 0;

    [Header("Turn Off")]
    public int offHour = 0;
    public int offMinute = 0;

    bool state = false;


    // Update is called once per frame
    private void LateUpdate()
    {
        int curHour = SceneTime.time.Hour;
        int curMinute = SceneTime.time.Minute;

        if (!state && curHour == onHour && (curMinute == onMinute || curMinute > onMinute))
        {
            state = true;

            Switch(transform, state);

        }
        else if (state && curHour == offHour && (curMinute == offMinute || curMinute > offMinute))
        {
            state = false;

            Switch(transform, state);
        }
    }

    static void Switch(Transform transform, bool state)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(state);
        }
    }
}
