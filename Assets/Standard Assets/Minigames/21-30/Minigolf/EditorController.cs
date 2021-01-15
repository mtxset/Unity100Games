#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Minigames.Minigolf {
    [CustomEditor(typeof(PowerController))]
    public class EditorController : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var myScript = (PowerController)target;
            if (GUILayout.Button("Move Left")) {
                myScript.RotateBar(-1);
            } else if (GUILayout.Button("Move Right")) {
                myScript.RotateBar(1);
            } else if (GUILayout.Button("Increase Power")) {
                myScript.IncreasePower();
            } else if (GUILayout.Button("Shoot")) {
                myScript.Shoot();
            }
         }
    }
}
#endif