#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Minigames.DoubleTrouble;

[CustomEditor(typeof(Guitar))]
public class GuitarEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var myScript = (Guitar)target;
        if (GUILayout.Button("hE"))     myScript.PlayString(0);
        else if (GUILayout.Button("B")) myScript.PlayString(1);
        else if (GUILayout.Button("G")) myScript.PlayString(2);
        else if (GUILayout.Button("D")) myScript.PlayString(3);
        else if (GUILayout.Button("A")) myScript.PlayString(4);
        else if (GUILayout.Button("E")) myScript.PlayString(5);
        else if (GUILayout.Button("PICK")) myScript.PlayStringWherePick();
    }
}
#endif