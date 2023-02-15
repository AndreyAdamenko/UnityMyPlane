using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AirFriction)), CanEditMultipleObjects]
public class AirFrictionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        bool getScale = GUILayout.Button("Get transform scale");

        if (getScale)
        {
            AirFriction targetScript = (AirFriction)target;

            targetScript.GetScale();

            Repaint();
        }
    }

}
