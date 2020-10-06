using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Menu))]
public class EditorController : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var myScript = (Menu)target;
        if (GUILayout.Button("Next Page")) {
            myScript.NextPage();
        } else if (GUILayout.Button("Previous Page")) {
            myScript.PreviousPage();
        }
    }
}