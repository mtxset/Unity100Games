#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Components.UnityComponents;

[CustomEditor(typeof(BasicMenu))]
public class BasicMenuEditorController : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var myScript = (BasicMenu)target;
        if (GUILayout.Button("Next Page")) {
            myScript.Menu.NextPage();
        } else if (GUILayout.Button("Previous Page")) {
            myScript.Menu.PreviousPage();
        } else if (GUILayout.Button("Next Item")) {
            myScript.Menu.NextMenuItem();
        } else if (GUILayout.Button("Previous Item")) {
            myScript.Menu.PreviousMenuItem();
        } else if (GUILayout.Button("Select")) {
            myScript.Menu.Select();
        }
    }
}
#endif