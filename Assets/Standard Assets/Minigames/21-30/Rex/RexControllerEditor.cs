#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Minigames.Rex;

[CustomEditor(typeof(PlayerController))]
public class RexControllerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var myScript = (PlayerController)target;
        if (GUILayout.Button("Jump"))
            myScript.Jump();
    }
}
#endif