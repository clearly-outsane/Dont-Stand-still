using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FOVDebug : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.green;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

        Handles.color = Color.red;
        foreach(Transform aggroTarget in fov.aggroTargets)
        {
            Handles.DrawLine(fov.transform.position, aggroTarget.position);
        }
    }
}
