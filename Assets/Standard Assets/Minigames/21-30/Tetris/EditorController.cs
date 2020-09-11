using UnityEngine;
using UnityEditor;

namespace Minigames.Tetris
{

    // [CustomEditor(typeof(TetrisBlockController))]
    // public class EditorController : Editor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         DrawDefaultInspector();

    //         var myScript = (TetrisBlockController)target;
    //         if (GUILayout.Button("Move Left"))
    //         {
    //             myScript.MoveCurrentBlock(-1);
    //         } else if (GUILayout.Button("Move Right")) {
    //             myScript.MoveCurrentBlock(1);
    //         } else if (GUILayout.Button("Drop")) {
    //             myScript.DropBlock();
    //         } else if (GUILayout.Button("Reset")) {
    //             myScript.FallRate = 1;
    //         } else if (GUILayout.Button("Rotate")) {
    //             myScript.Rotate();
    //         }
    //      }
    // }

    // [CustomEditor(typeof(Spawner))]
    // public class SpawnerEditorController : Editor {
    //     public override void OnInspectorGUI() {
    //         DrawDefaultInspector();

    //         var spawner = (Spawner)target;

    //         if (GUILayout.Button("Spawn")) spawner.SpawnNewPiece();
    //     }
    // }
}

