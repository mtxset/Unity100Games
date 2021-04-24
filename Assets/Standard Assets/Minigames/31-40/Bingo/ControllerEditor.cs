#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Minigames.Bingo;

[CustomEditor(typeof(BingoTable))]
public class ControllerEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var myScript = (BingoTable)target;
        if (GUILayout.Button("LEFT"))
            myScript.Traverse(0, -1);
        else if (GUILayout.Button("RIGHT"))
            myScript.Traverse(0, 1);
        else if (GUILayout.Button("UP"))
            myScript.Traverse(1, 0);
        else if (GUILayout.Button("DOWN"))
            myScript.Traverse(-1, 0);
        else if (GUILayout.Button("Spawn"))
            myScript.SpawnNewFallingNumber();
        else if (GUILayout.Button("Select"))
            myScript.Select();
    }
}
#endif