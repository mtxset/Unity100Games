#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Minigames.UDPClient;

[CustomEditor(typeof(UDPClient))]
public class UDPClientEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var myScript = (UDPClient)target;
        if (GUILayout.Button("Send")) {
            myScript.sendUDPMessage();
        }
    }
}
#endif
